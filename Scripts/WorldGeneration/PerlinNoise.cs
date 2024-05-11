using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Transactions;
public partial class PerlinNoise : Node
{
    #region Noise
    [Export]
    public WorldGenSettings settings;
    [Export]
    bool MainMenu;
    Random rand;
    FastNoiseLite moisture = new FastNoiseLite();
    FastNoiseLite altitude = new FastNoiseLite();
    FastNoiseLite temperature = new FastNoiseLite();
    #endregion
    private float[,] altitudeMap;
    private float[,] temperatureMap;
    private float[,] moistureMap;
    private string[,] biomeMap;
    AssetManager assets;
    GameManager gameManager;
    [Export]
    TileMap tileMap;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Starting generation");
        assets = (AssetManager)GetNode("/root/AssetManager");
        gameManager = (GameManager)GetNode("/root/GameManager");
        if(MainMenu)
        {
            settings = new WorldGenSettings(2048, 1, 0.001f, 4, 2, 10);
        }
        else
        {
            GD.Print("Loading world");
            settings = gameManager.CurrentWorld.Settings;
        }
        
        tileMap.TileSet = assets.GetWorldTileSet();
        if (settings.Seed == 0)
        {
            Random seedGen = new Random();
            int seed = seedGen.Next(999999999);
            settings.Seed = seed;
            gameManager.CurrentWorld.Settings.Seed = seed;
            rand = new Random(seed);
            gameManager.SaveCurrentWorld();
        }
        else
        {
            rand = new Random(settings.Seed);
        }
        moisture.Seed = rand.Next();
        altitude.Seed = rand.Next();
        temperature.Seed = rand.Next();
        moisture.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        moisture.FractalLacunarity = settings.Lacunarity;
        moisture.Frequency = settings.Frequency;
        moisture.FractalOctaves = settings.Octaves;
        altitude.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        altitude.FractalLacunarity = settings.Lacunarity;
        altitude.Frequency = settings.Frequency;
        altitude.FractalOctaves = settings.Octaves;
        temperature.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        temperature.FractalLacunarity = settings.Lacunarity;
        temperature.Frequency = settings.Frequency;
        temperature.FractalOctaves = settings.Octaves;
        moisture.FractalGain = altitude.FractalGain = temperature.FractalGain = 1.05f;
        // Initialize world data arrays
        altitudeMap = new float[settings.Size, settings.Size];
        temperatureMap = new float[settings.Size, settings.Size];
        moistureMap = new float[settings.Size, settings.Size];
        biomeMap = new string[settings.Size, settings.Size];
        GenerateWorldData();
        AssignBiomes();
        ProcessGrid();
        SeparateBiomesWithRivers();
        DrawBiomes();
        gameManager.SetCurrentWorldBiomeMap(biomeMap);
        GD.Print(gameManager.currentWorldBiomeMap.Length);
        if(gameManager.CurrentState == "TRAVEL")
        {
            var scene = GD.Load<PackedScene>("res://Templates/Player.tscn");
            var player = scene.Instantiate();
            CallDeferred("add_sibling", player);
        }


    }

    /// <summary>
    /// Generates basic world data: moisture, altitude, temperature
    /// </summary>
    void GenerateWorldData()
    {
        int equatorY = settings.Size / 2;
        for (int x = 0; x < settings.Size; x++)
        {
            for (int y = 0; y < settings.Size; y++)
            {
                altitudeMap[x, y] = NormalizeValue(altitude.GetNoise2D(x, y));
                moistureMap[x, y] = NormalizeValue(moisture.GetNoise2D(x, y));
                float equatorDistance = Math.Abs((float)(y - equatorY) / equatorY);
                equatorDistance = Math.Min(equatorDistance, 1.0f);

                // Combine the base temperature from the gradient and the noise value
                // Here, weight the latitudinal gradient more heavily than the noise to ensure
                // the desired pattern is dominant while still allowing for variation
                float baseTemperature = 1.0f - equatorDistance;  // equator is hottest (1.0), poles coldest (0.0)
                float noiseValue = NormalizeValue(temperature.GetNoise2D(x, y));

                // Blend the base temperature with the noise value
                // You can adjust the weights according to your desired level of noise influence
                float weightBase = 0.7f; // Adjust this value as needed
                float weightNoise = 0.3f; // Adjust this value as needed
                temperatureMap[x, y] = weightBase * baseTemperature + weightNoise * noiseValue;
            }
        }

    }


    void DrawBiomes()
    {

        for (int x = 0; x < settings.Size; x++)
        {
            for (int y = 0; y < settings.Size; y++)
            {
                tileMap.SetCell(0, new Vector2I(x, y), assets.GetBiome(biomeMap[x, y]).TilesetSourceId, Vector2I.Zero, 0);
            }
        }


    }
    public void PrintBiomes()
    {

        for (int x = 0; x < settings.Size; x++)
        {
            for (int y = 0; y < settings.Size; y++)
            {
                GD.Print(biomeMap[x, y]);
            }
        }
    }

    /// <summary>
    /// Assigns the biomes depending on their moisture and temperature range
    /// </summary>
    void AssignBiomes()
    {
        Dictionary<string, Biome> biomes = assets.GetBiomes();
        for (int x = 0; x < settings.Size; x++)
        {
            for (int y = 0; y < settings.Size; y++)
            {
                Biome chosenBiome = null;
                float maxScore = float.MinValue;
                foreach (var biome in biomes)
                {
                    if (moistureMap[x, y] >= biome.Value.MoistureRange[0] && moistureMap[x, y] <= biome.Value.MoistureRange[1] &&
                        temperatureMap[x, y] >= biome.Value.TemperatureRange[0] && temperatureMap[x, y] <= biome.Value.TemperatureRange[1] &&
                        altitudeMap[x, y] >= biome.Value.AltitudeRange[0] && altitudeMap[x, y] <= biome.Value.AltitudeRange[1])
                    {
                        float score = biome.Value.Priority;

                        if (score > maxScore)
                        {
                            maxScore = score;
                            chosenBiome = biome.Value;
                        }
                    }
                }

                // Assign chosen biome to the cell
                biomeMap[x, y] = chosenBiome != null ? chosenBiome.ID : "Biome_Ocean";
            }
        }

    }


    /// <summary>
    /// Removes biomes that are too small and incorporates them into the bigger neighboring biome
    /// </summary>
    /// 

    void ProcessGrid()
    {
        int rows = settings.Size;
        int cols = settings.Size;
        bool[,] visited = new bool[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (!visited[i, j])
                {
                    string currentValue = biomeMap[i, j];
                    List<(int, int)> currentClump = new List<(int, int)>();
                    int clumpSize = 0;

                    // Use a flood fill (DFS) to find the clump and its size
                    FloodFill(biomeMap, visited, i, j, currentValue, currentClump, ref clumpSize);

                    // Check if clump size is less than the minimum size
                    if (clumpSize < settings.MinBiomeSize)
                    {
                        // Find the largest adjacent clump value
                        string largestAdjacentValue = FindLargestAdjacentClump(biomeMap, visited, i, j, currentValue, rows, cols);

                        // Change the value of the current clump to the largest adjacent value
                        foreach (var coord in currentClump)
                        {
                            biomeMap[coord.Item1, coord.Item2] = largestAdjacentValue;
                        }
                    }
                }
            }
        }
    }
    // Direction vectors for up, down, left, right, and diagonals
    readonly int[] dRow = { -1, 1, 0, 0, -1, -1, 1, 1 };
    readonly int[] dCol = { 0, 0, -1, 1, -1, 1, -1, 1 };
    void FloodFill(string[,] grid, bool[,] visited, int row, int col, string value,
                          List<(int, int)> currentClump, ref int clumpSize)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Create a stack for the flood fill algorithm
        Stack<(int, int)> stack = new Stack<(int, int)>();
        stack.Push((row, col));
        visited[row, col] = true;

        // Process the stack
        while (stack.Count > 0)
        {
            var (currentRow, currentCol) = stack.Pop();
            currentClump.Add((currentRow, currentCol));
            clumpSize++;

            // Check neighbors
            for (int i = 0; i < 8; i++)
            {
                int newRow = currentRow + dRow[i];
                int newCol = currentCol + dCol[i];

                // If neighbor is within bounds, has the same value, and hasn't been visited yet
                if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols &&
                    !visited[newRow, newCol] && grid[newRow, newCol] == value)
                {
                    stack.Push((newRow, newCol));
                    visited[newRow, newCol] = true;
                }
            }
        }
    }

    string FindLargestAdjacentClump(string[,] grid, bool[,] visited, int row, int col,
                                           string currentValue, int rows, int cols)
    {
        Dictionary<string, int> adjacentClumps = new Dictionary<string, int>();

        // Check all 8 directions
        for (int i = 0; i < 8; i++)
        {
            int newRow = row + dRow[i];
            int newCol = col + dCol[i];

            // Check if the neighbor is within bounds
            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
            {
                string neighborValue = grid[newRow, newCol];

                // If neighbor is not the same as the current value and has been visited
                if (neighborValue != currentValue && visited[newRow, newCol])
                {
                    // Add or update the count of the neighbor's value
                    if (adjacentClumps.ContainsKey(neighborValue))
                    {
                        adjacentClumps[neighborValue]++;
                    }
                    else
                    {
                        adjacentClumps[neighborValue] = 1;
                    }
                }
            }
        }

        // Find the largest adjacent clump
        string largestAdjacentValue = "";
        int maxCount = 0;

        foreach (var clump in adjacentClumps)
        {
            if (clump.Value > maxCount)
            {
                maxCount = clump.Value;
                largestAdjacentValue = clump.Key;
            }
        }

        return largestAdjacentValue;
    }
    void SeparateBiomesWithRivers()
    {
        int riverWidth = 3;
        int width = settings.Size;
        int height = settings.Size;

        // Create a copy of the original array to keep track of which cells will be converted to rivers
        string[,] biomeCopy = (string[,])biomeMap.Clone();

        // Iterate over the array and find biome boundaries
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (biomeMap[i,j] == "Biome_River" || biomeMap[i, j] == "Biome_Ocean")
                {
                    continue;
                }

                // Check horizontal neighbor
                if (j < width - 1 && biomeMap[i, j] != biomeMap[i, j + 1] && biomeMap[i, j + 1] != "Biome_Ocean")
                {
                    // Mark cells as river
                    for (int k = 0; k < riverWidth; k++)
                    {
                        if (j + k < width)
                        {
                            biomeCopy[i, j + k] = "Biome_River";
                        }
                    }
                }

                // Check vertical neighbor
                if (i < height - 1 && biomeMap[i, j] != biomeMap[i + 1, j] && biomeMap[i + 1, j] != "Biome_Ocean")
                {
                    // Mark cells as river
                    for (int k = 0; k < riverWidth; k++)
                    {
                        if (i + k < height)
                        {
                            biomeCopy[i + k, j] = "Biome_River";
                        }
                    }
                }
            }
        }

        // Copy the modified biomeCopy back to the original array
        Array.Copy(biomeCopy, biomeMap, biomeMap.Length);
    }

    /// <summary>
    /// For normalizing the values from the noise function from [-1;1] to [0;1]
    /// </summary>
    /// <param name="value">Float values in range of -1 to 1</param>
    /// <returns></returns>
    static float NormalizeValue(float value)
    {
        return (value + 1) / 2;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

}

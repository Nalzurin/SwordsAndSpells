using Godot;
using System;
using System.Collections.Generic;
public partial class PerlinNoise : Node
{
    #region Noise
    [Export]
    public WorldGenSettings settings;

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
    [Export]
    TileMap tileMap;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        assets = (AssetManager)GetNode("/root/AssetManager");
        tileMap.TileSet = assets.GetWorldTileSet();
        if (settings.Seed == 0)
        {
            rand = new Random();
        }
        else
        {
            rand = new Random(settings.Seed);
        }
        moisture.Seed = rand.Next();
        altitude.Seed = rand.Next();
        temperature.Seed = rand.Next();
        moisture.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        altitude.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        temperature.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        // Initialize world data arrays
        altitudeMap = new float[settings.Size, settings.Size];
        temperatureMap = new float[settings.Size, settings.Size];
        moistureMap = new float[settings.Size, settings.Size];
        biomeMap = new string[settings.Size, settings.Size];
        GenerateWorldData();
        AssignBiomes();
        DrawBiomes();
    }

    /// <summary>
    /// Generates basic world data: moisture, altitude, temperature
    /// </summary>
    void GenerateWorldData()
    {
        for (int x = 0; x < settings.Size; x++)
        {
            for (int y = 0; y < settings.Size; y++)
            {
                altitudeMap[x, y] = NormalizeValue(altitude.GetNoise2D(x, y));
                temperatureMap[x, y] = NormalizeValue(temperature.GetNoise2D(x, y));
                moistureMap[x, y] = NormalizeValue(moisture.GetNoise2D(x, y));
                // Determine temperature deviation based on position
                //float centerY = settings.Size / 2f;
                //float deviation = Math.Abs(y - centerY) / centerY;
                //temperatureMap[x, y] += deviation * temperatureGradient.y;

            }
        }
    }


    void DrawBiomes()
    {
        
        for (int x = 0; x < settings.Size; x++)
        {
            for (int y = 0; y < settings.Size; y++)
            {
                tileMap.SetCell(0, new Vector2I(x,y), assets.GetBiome(biomeMap[x, y]).TilesetSourceId, Vector2I.Zero, 0);
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
                        temperatureMap[x, y] >= biome.Value.TemperatureRange[0] && temperatureMap[x, y] <= biome.Value.TemperatureRange[1])
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
    private void FloodFillBiomes()
    {
        for (int x = 0; x < settings.Size; x++)
        {
            for (int y = 0; y < settings.Size; y++)
            {
                if (IsSmallBiome(x, y))
                {
                    MergeBiome(x, y);
                }
            }
        }
    }

    private bool IsSmallBiome(int startX, int startY)
    {
        string targetBiome = biomeMap[startX, startY];
        int size = 1;
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(new Vector2(startX, startY));
        HashSet<Vector2> visited = new HashSet<Vector2>();
        visited.Add(new Vector2(startX, startY));

        while (queue.Count > 0)
        {
            Vector2 current = queue.Dequeue();

            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                int x = (int)neighbor.X;
                int y = (int)neighbor.Y;

                if (biomeMap[x, y] == targetBiome && !visited.Contains(neighbor))
                {
                    size++;
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        return size < settings.MinBiomeSize;
    }

    private void MergeBiome(int startX, int startY)
    {
        string targetBiome = biomeMap[startX, startY];
        string replacementBiome = GetMostCommonBiome(startX, startY);
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(new Vector2(startX, startY));
        HashSet<Vector2> visited = new HashSet<Vector2>();
        visited.Add(new Vector2(startX, startY));

        while (queue.Count > 0)
        {
            Vector2 current = queue.Dequeue();
            int x = (int)current.X;
            int y = (int)current.Y;

            // Set the new biome
            biomeMap[x, y] = replacementBiome;

            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                int nx = (int)neighbor.X;
                int ny = (int)neighbor.Y;

                if (biomeMap[nx, ny] == targetBiome && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }
    }

    private string GetMostCommonBiome(int x, int y)
    {
        Dictionary<string, int> biomeCounts = new Dictionary<string, int>();
        foreach (var biome in assets.GetBiomes())
        {
            biomeCounts[biome.Key] = 0;
        }

        foreach (Vector2 neighbor in GetNeighbors(new Vector2(x, y)))
        {
            int nx = (int)neighbor.X;
            int ny = (int)neighbor.Y;
            string biome = biomeMap[nx, ny];
            biomeCounts[biome]++;
        }

        string mostCommonBiome = "Biome_TemperateForest"; // Default biome
        int maxCount = 0;

        foreach (var kvp in biomeCounts)
        {
            if (kvp.Value > maxCount)
            {
                mostCommonBiome = kvp.Key;
                maxCount = kvp.Value;
            }
        }

        return mostCommonBiome;
    }

    private List<Vector2> GetNeighbors(Vector2 position)
    {
        int x = (int)position.X;
        int y = (int)position.Y;

        List<Vector2> neighbors = new List<Vector2>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx != 0 || dy != 0)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && nx < settings.Size && ny >= 0 && ny < settings.Size)
                    {
                        neighbors.Add(new Vector2(nx, ny));
                    }
                }
            }
        }

        return neighbors;
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

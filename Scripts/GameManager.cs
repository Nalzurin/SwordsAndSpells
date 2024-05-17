using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
    [Signal]
    public delegate void UpdateEntitiesEventHandler(Vector2I playerPos);
    [Signal]
    public delegate void StateChangedEventHandler(string newState);

    public string CurrentState;
    public string[,] currentWorldBiomeMap;
    private AssetManager assetManager;
    public WorldFile CurrentWorld;
    public PlayerEntity CurrentPlayer;
    public EnemyController CurrentEnemy;
    private SaveSystem SaveSystem;
    public AStarGrid2D pathfinding = new AStarGrid2D();
    public int CurrentEnemyCount = 0;
    public EnemyEntity newEnemy;
    public Vector2I newEnemyPos;
    List<EnemyEntity> possibleEntities = new List<EnemyEntity>();
    PackedScene combatScene = GD.Load<PackedScene>("Scenes/CombatGUI.tscn");
    PackedScene travelScene = GD.Load<PackedScene>("Scenes/TravelGUI.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SaveSystem = (SaveSystem)GetNode("/root/SaveSystem");
        assetManager = (AssetManager)GetNode("/root/AssetManager");
        CurrentState = "MENU";
    }
    public void BeginTravel()
    {
        var travelInstance = travelScene.Instantiate();
        GetNode("/root/World").AddChild(travelInstance);
    }
    public void BeginCombat(EnemyController enemy)
    {
        SwitchState("COMBAT");
        CurrentEnemy = enemy;
        var combatInstance = combatScene.Instantiate();
        GetNode("/root/World").AddChild(combatInstance);

    }
    public void EndCombat()
    {
        SaveSystem.SaveCharacter(CurrentPlayer);
        SwitchState("TRAVEL");
        BeginTravel();
    }
    public BaseItem GenerateLoot(int level, string enemyRarity)
    {
        List<BaseItem> items = new List<BaseItem>();
        foreach(BaseItem item in assetManager.items.Values)
        {
            if (item.ItemLevel > level - 5 && item.ItemLevel < level + 5)
            {
                if(item.Rarity == enemyRarity)
                {
                    items.Add(item);
                }
            }
        }
        Random rand = new Random();
        return items[rand.Next(items.Count)];
    }
    public override void _Process(double delta)
    {

    }
    public void EmitSignalEntitiesUpdate(Vector2I currentPos)
    {
        Random random = new Random();
        EmitSignal(SignalName.UpdateEntities, currentPos);
        foreach (EnemyEntity enemy in assetManager.enemies.Values)
        {
            if (enemy.Characteristics.CharacterLevel <= CurrentPlayer.Characteristics.CharacterLevel + 5 && enemy.Characteristics.CharacterLevel >= CurrentPlayer.Characteristics.CharacterLevel - 5)
            {
                possibleEntities.Add(enemy);
            }
        }
        if (CurrentEnemyCount < 30 && random.Next(1,100) >= 75)
        {
            InstantiateEnemy(currentPos, 30, 40);
        }
       
    }
    public EnemyEntity RandomEntity()
    {
        List<EnemyEntity> entities = new List<EnemyEntity>();
        foreach(EnemyEntity entity in assetManager.enemies.Values)
        {
            if (entity.Characteristics.CharacterLevel <= CurrentPlayer.Characteristics.CharacterLevel + 5 && entity.Characteristics.CharacterLevel >= CurrentPlayer.Characteristics.CharacterLevel - 5)
            {
                entities.Add(entity);
            }
        }
        Random rand = new Random();
        EnemyEntity enemy = entities[rand.Next(entities.Count)];
        
        return enemy;
    }
    public void InstantiateEnemy(Vector2I playerPosition, int minDistance, int maxDistance)
    {
        Dictionary<Vector2I, bool> checkedTiles = new Dictionary<Vector2I, bool>();
        Random rand = new Random();
        Vector2I validPosition = FindValidPosition(playerPosition, minDistance, maxDistance, checkedTiles, rand);

        if (validPosition != null)
        {
            newEnemyPos = validPosition;
            newEnemy = RandomEntity();
            var scene = GD.Load<PackedScene>("res://Templates/Enemy.tscn");
            var enemy = scene.Instantiate();
            GetNode("/root/World").CallDeferred("add_child", enemy);
            CurrentEnemyCount++;
        }
    }

    private Vector2I FindValidPosition(Vector2I playerPosition, int minDistance, int maxDistance, Dictionary<Vector2I, bool> checkedTiles, Random rand)
    {
        Vector2I validPosition = Vector2I.Zero;

        // Calculate total number of tiles in the search area
        int totalTiles = (2 * maxDistance + 1) * (2 * maxDistance + 1);

        // Generate random order to check tiles
        List<Vector2I> randomTiles = GenerateRandomTiles(playerPosition, minDistance, maxDistance, rand);

        foreach (Vector2I tile in randomTiles)
        {
            // Check if tile has already been checked
            if (!checkedTiles.ContainsKey(tile))
            {
                // Mark tile as checked
                checkedTiles[tile] = true;

                // Check if tile is within bounds and is walkable
                if (tile.X >= 0 && tile.X < currentWorldBiomeMap.GetLength(0) &&
                    tile.Y >= 0 && tile.Y < currentWorldBiomeMap.GetLength(1) &&
                    assetManager.biomes[currentWorldBiomeMap[tile.X, tile.Y]].IsWalkable &&
                    !CurrentWorld.SavedEntities.ContainsKey(tile))
                {
                    validPosition = tile;
                    break;
                }
            }
        }

        return validPosition;
    }

    private List<Vector2I> GenerateRandomTiles(Vector2I playerPosition, int minDistance, int maxDistance, Random rand)
    {
        List<Vector2I> randomTiles = new List<Vector2I>();
        HashSet<Vector2I> visitedTiles = new HashSet<Vector2I>();

        // Generate random order of tiles to check
        for (int distance = minDistance; distance <= maxDistance; distance++)
        {
            for (int i = 0; i < 4; i++) // Repeat 4 times to cover all directions
            {
                double angle = rand.NextDouble() * Math.PI / 2 + i * Math.PI / 2; // Random angle in multiples of 90 degrees
                int offsetX = (int)(distance * Math.Cos(angle));
                int offsetY = (int)(distance * Math.Sin(angle));
                Vector2I tile = new Vector2I(playerPosition.X + offsetX, playerPosition.Y + offsetY);

                if (!visitedTiles.Contains(tile))
                {
                    randomTiles.Add(tile);
                    visitedTiles.Add(tile);
                }
            }
        }

        // Shuffle the list to randomize the order
        for (int i = 0; i < randomTiles.Count; i++)
        {
            int randomIndex = rand.Next(i, randomTiles.Count);
            Vector2I temp = randomTiles[i];
            randomTiles[i] = randomTiles[randomIndex];
            randomTiles[randomIndex] = temp;
        }

        return randomTiles;
    }
    public void UpdatePathfinding()
    {
        pathfinding.DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan;
        pathfinding.DefaultEstimateHeuristic = AStarGrid2D.Heuristic.Manhattan;
        pathfinding.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
        pathfinding.CellSize = new Vector2I(16, 16);
        pathfinding.Update();
        pathfinding.Region = new Rect2I(0, 0, CurrentWorld.Settings.Size, CurrentWorld.Settings.Size);
        pathfinding.Update();
        for (int x = 0; x < CurrentWorld.Settings.Size; x++)
        {
            for (int y = 0; y < CurrentWorld.Settings.Size; y++)
            {
                if (assetManager.GetBiome(currentWorldBiomeMap[x, y]).IsWalkable == false)
                {
                    pathfinding.SetPointSolid(new Vector2I(x, y), true);
                }
            }
        }
        pathfinding.Update();
    }
    public void SetPlayer(PlayerEntity player)
    {
        CurrentPlayer = player;
        GD.Print("Current Player: " + CurrentPlayer.EntityName);
    }
    public void SetWorld(WorldFile world)
    {

        CurrentWorld = world;

    }
    public void SaveCurrentPlayer()
    {
        SaveSystem.SaveCharacter(CurrentPlayer);
    }
    public void SaveCurrentWorld()
    {
        SaveSystem.SaveWorld(CurrentWorld);
    }
    public void SwitchState(string newState)
    {
        CurrentState = newState;
        EmitSignal(SignalName.StateChanged, newState);
    }
    public void SetCurrentWorldBiomeMap(string[,] worldBiomeMap)
    {
        currentWorldBiomeMap = worldBiomeMap;

    }
    public Biome GetBiome(Vector2I pos)
    {
        return assetManager.GetBiome(currentWorldBiomeMap[pos.X, pos.Y]);
    }
    public Biome GetBiome(int x, int y)
    {
        return assetManager.GetBiome(currentWorldBiomeMap[x, y]);
    }

}

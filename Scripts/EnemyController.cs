using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class EnemyController : Node2D
{
	[Export]
	public EnemyEntity EnemyData;
    [Export]
    Sprite2D EnemySprite;
	// Called when the node enters the scene tree for the first time.
	GameManager GameManager;
    TileMap _tileMap;
    bool IsPlayerInRange = false;
    Random random = new Random();
    Node2D player;
    public override void _Ready()
	{
        GameManager = (GameManager)GetNode("/root/GameManager");
       
        _tileMap = (TileMap)GetNode("/root/World/TileMap");
        GameManager.UpdateEntities += AI_Move;
        EnemyData = GameManager.newEnemy;
        Position = _tileMap.MapToLocal(GameManager.newEnemyPos);
        GameManager.CurrentWorld.SavedEntities.Add(_tileMap.LocalToMap(Position), EnemyData);
        EnemySprite.Texture = EnemyData.GetSprite();
    }
    public void DeleteSelf()
    {
        GD.Print("Enemy: Deleting Self");
        GameManager.UpdateEntities -= AI_Move;
        GameManager.CurrentEnemyCount--;
        GameManager.CurrentWorld.SavedEntities.Remove((Vector2I)Position);
        QueueFree();
    }
    private void SetPosition(Vector2I position)
    {
        Position = position;
    }
    private void AI_Move(Vector2I playerPos)
    {
        if (Math.Abs((playerPos - _tileMap.LocalToMap((Vector2I)Position)).Length()) > 50)
        {
            GD.Print((playerPos - _tileMap.LocalToMap((Vector2I)Position)).Length());
            DeleteSelf();
            return;
        }
        Vector2I prevPos = (Vector2I)Position;

        if (!IsPlayerInRange)
        {
            GD.Print("Enemy:Moving Randomly");
            Move(randomDirection());
        }
        else
        {
            GD.Print("Enemy:Moving towards player");
            Move(GetPlayerDirection());
        }
        //GameManager.CurrentWorld.SavedEntities.Add(_tileMap.LocalToMap((Vector2I)Position),EnemyData);

            GameManager.CurrentWorld.SavedEntities.Remove(_tileMap.LocalToMap((Vector2I)prevPos));
    }
    public Vector2I GetPlayerDirection()
    {
        Array<Vector2I> path = GameManager.pathfinding.GetIdPath(_tileMap.LocalToMap((Vector2I)Position), _tileMap.LocalToMap(player.Position));
        foreach(Vector2I pos in path) { GD.Print(pos.ToString()); }
        if (path.Count <= 2)
        {
            GameManager.BeginCombat(this);
            return Vector2I.Zero;
            
        }
        else
        {
            GD.Print(path[1] - _tileMap.LocalToMap(Position));
            return path[1]-_tileMap.LocalToMap(Position);

        }

    }
    public void OnAreaEntered(Area2D area)
    {
        if (area.GetParent().IsInGroup("Player"))
        {
            player = (Node2D)area.GetParent();
            IsPlayerInRange = true;
        }

    }
    public void OnAreaExited(Area2D area)
    {
        if (area.GetParent().IsInGroup("Player"))
        {
            IsPlayerInRange = false;
        }
    }

    private Vector2I randomDirection()
    {
        return new Vector2I(random.Next(0, 3) - 1, random.Next(0,3)-1);
    }

    public void Move(Vector2I direction)
    {
        // Calculate the target position in tile coordinates
        Vector2I currentTilePos = _tileMap.LocalToMap(Position);
        Vector2I targetTilePos = currentTilePos + direction;

        // Check if the target position is within the tilemap bounds and is walkable
        if (IsMoveValid(targetTilePos))
        {
            // Convert the target tile position back to world position
            Vector2 targetWorldPos = _tileMap.MapToLocal(targetTilePos);

            // Update player's position to the target position
            Position = targetWorldPos;
        }
    }

    private bool IsMoveValid(Vector2I targetTilePos)
    {
        // Check if the target position is within the tilemap bounds
        if (targetTilePos.X < GameManager.CurrentWorld.Settings.Size && targetTilePos.X >= 0 && targetTilePos.Y >= 0 && targetTilePos.Y < GameManager.CurrentWorld.Settings.Size && GameManager.currentWorldBiomeMap[targetTilePos.X, targetTilePos.Y] != null && GameManager.GetBiome(targetTilePos.X, targetTilePos.Y).IsWalkable == true)
        {
            // Check if the target position is not already occupied by another enemy
            if (!GameManager.CurrentWorld.SavedEntities.ContainsKey(targetTilePos))
            {
                // Mark the target position as occupied
                GameManager.CurrentWorld.SavedEntities[targetTilePos] = EnemyData;
                return true;
            }
        }
        return false;
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

}

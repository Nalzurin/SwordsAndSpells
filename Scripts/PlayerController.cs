using Godot;
using System;

public partial class PlayerController : Node2D
{
	[Export]
	public PlayerEntity PlayerData;
    [Export]
    Sprite2D PlayerSprite;
	// Called when the node enters the scene tree for the first time.
	GameManager GameManager;
    TileMap _tileMap;
	public override void _Ready()
	{
        GameManager = (GameManager)GetNode("/root/GameManager");
       
        _tileMap = (TileMap)GetNode("/root/World/TileMap");
        PlayerData = GameManager.CurrentPlayer;
        PlayerSprite.Texture = PlayerData.GetSprite();
        SetStartingPosition();

    }
    private void SetStartingPosition()
    {
        if (GameManager.CurrentWorld.WorldSpawn != Vector2I.Zero)
        {
            Position =  GameManager.CurrentWorld.WorldSpawn;
        }
        else
        {
            Position =  randomLocation();
        }
    }
    private Vector2 randomLocation()
    {
        Random random = new Random();
        bool IsCorrect = false;
        Vector2I pos = new Vector2I();
        while (!IsCorrect)
        {
            pos = new Vector2I(random.Next(0, GameManager.CurrentWorld.Settings.Size-1), random.Next(0, GameManager.CurrentWorld.Settings.Size-1));
            if (GameManager.GetBiome(pos.X, pos.Y).IsWalkable == true)
            {
                IsCorrect = true;
                GameManager.CurrentWorld.WorldSpawn = pos;
                GameManager.SaveCurrentWorld();
            }
        }
        return _tileMap.MapToLocal(pos);

    }
    private bool IsMovementInputPressed(Vector2I direction)
    {
        // Determine input actions based on direction
        if (direction == Vector2I.Up && (Input.IsActionPressed("ui_up") || Input.IsActionPressed("move_up")))
        {
            return true;
        }
        if (direction == Vector2I.Down && (Input.IsActionPressed("ui_down") || Input.IsActionPressed("move_down")))
        {
            return true;
        }
        if (direction == Vector2I.Left && (Input.IsActionPressed("ui_left") || Input.IsActionPressed("move_left")))
        {
            return true;
        }
        if (direction == Vector2I.Right && (Input.IsActionPressed("ui_right") || Input.IsActionPressed("move_right")))
        {
            return true;
        }
        return false;
    }
    public void Move(Vector2I direction)
    {
        // Calculate the target position in tile coordinates
        Vector2I currentTilePos = _tileMap.LocalToMap(Position);
        Vector2I targetTilePos = currentTilePos + direction;
        GD.Print(currentTilePos);
        // Check if the target position is within the tilemap bounds
        if (targetTilePos.X < GameManager.CurrentWorld.Settings.Size && targetTilePos.X >= 0 && targetTilePos.Y >= 0 && targetTilePos.Y < GameManager.CurrentWorld.Settings.Size && GameManager.currentWorldBiomeMap[currentTilePos.X, currentTilePos.Y] != null  && GameManager.GetBiome(targetTilePos.X, targetTilePos.Y).IsWalkable == true)
        {
            GD.Print("Moving");
            // Convert the target tile position back to world position
            Vector2 targetWorldPos = _tileMap.MapToLocal(targetTilePos);

            // Update player's position to the target position
            Position = targetWorldPos;
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (GameManager.CurrentState == "TRAVEL")
        {

            if (Input.IsActionJustPressed("ui_up"))
            {
                GD.Print("trying to move up");
                Move(Vector2I.Up);
            }
            if (Input.IsActionJustPressed("ui_down"))
            {
                Move(Vector2I.Down);
            }
            if (Input.IsActionJustPressed("ui_left"))
            {
                Move(Vector2I.Left);
            }
            if (Input.IsActionJustPressed("ui_right"))
            {
                Move(Vector2I.Right);
            }
        }
    }

}

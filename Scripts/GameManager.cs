using Godot;
using System;

public partial class GameManager : Node
{

	public string CurrentState;
	public string[,] currentWorldBiomeMap;
	private AssetManager assetManager;
	public WorldFile CurrentWorld;
	public PlayerEntity CurrentPlayer;
	private SaveSystem SaveSystem;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SaveSystem = (SaveSystem)GetNode("/root/SaveSystem");
		assetManager = (AssetManager)GetNode("/root/AssetManager");
		CurrentState = "MENU";
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
	public void SaveCurrentWorld()
	{
		SaveSystem.SaveCharacter(CurrentPlayer);
	}
	public void SaveCurrentPlayer()
	{
		SaveSystem.SaveWorld(CurrentWorld);
	}
	public void SwitchState(string newState)
	{
		CurrentState = newState;
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

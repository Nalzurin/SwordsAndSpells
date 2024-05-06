using Godot;
using System;

public partial class GameManager : Node
{
	public string CurrentState { get; private set; }
	private Biome[,] currentWorldBiomeMap;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentState = "MENU";
	}
	public void SetCurrentWorldBiomeMap(Biome[,] worldBiomeMap)
	{
		currentWorldBiomeMap = worldBiomeMap;
	}
	public Biome GetBiome(Vector2I pos)
	{
		return currentWorldBiomeMap[pos.X, pos.Y];
	}
    public Biome GetBiome(int x, int y)
    {
        return currentWorldBiomeMap[x, y];
    }

}

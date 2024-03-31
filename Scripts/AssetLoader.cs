using Godot;
using System;

public partial class AssetLoader : Node
{
	BiomeLoader BiomeLoader;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BiomeLoader = new BiomeLoader();
		this.CallDeferred("add_sibling", BiomeLoader);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

using Godot;
using System;

public partial class AssetLoader : Node
{
	[Export]
	public string ScenePath;
	BiomeLoader BiomeLoader;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BiomeLoader = new BiomeLoader();
		this.CallDeferred("add_sibling", BiomeLoader);
		SwitchScene(ScenePath);

		
	}
    private void SwitchScene(string scenePath)
    {
        SceneTree sceneTree = GetTree();
        PackedScene scene = (PackedScene)ResourceLoader.Load(scenePath);
        sceneTree.CallDeferred("change_scene_to_packed", scene);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}

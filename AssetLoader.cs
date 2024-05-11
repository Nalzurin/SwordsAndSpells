using Godot;
using System;

public partial class AssetLoader : Node
{
	[Export]
	public string ScenePath;
	TilesetGenerator TilesetGenerator;
	// Called when the node enters the scene tree for the first time.
	SaveSystem SaveSystem;
	public override void _Ready()
	{
		SaveSystem = (SaveSystem)GetNode("/root/SaveSystem");

		BiomeLoader biomeLoader = new BiomeLoader();
		this.CallDeferred("add_sibling", biomeLoader);
		ItemLoader itemLoader = new ItemLoader();
		this.CallDeferred("add_sibling", itemLoader);
		AbilityLoader abilityLoader = new AbilityLoader();
		this.CallDeferred("add_sibling", abilityLoader);
		EnemyEntityLoader enemyEntityLoader = new EnemyEntityLoader();
        this.CallDeferred("add_sibling", enemyEntityLoader);
		CallDeferred("LoadCharacters");
        CallDeferred("LoadWorlds");
        TilesetGenerator = new TilesetGenerator();
        this.CallDeferred("add_sibling", TilesetGenerator);
        SwitchScene(ScenePath);

		
	}
	private void LoadCharacters()
	{
		SaveSystem.LoadCharacters();
	}
	private void LoadWorlds()
	{
		SaveSystem.LoadWorlds();
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

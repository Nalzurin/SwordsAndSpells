using Godot;
using System;

public partial class DebugPlayButton : Button
{
    [Export]
    string path;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Pressed += OnPress;
    }
    public void OnPress()
    {
        SwitchScene(path);
    }
    private void SwitchScene(string scenePath)
    {
        SceneTree sceneTree = GetTree();
        PackedScene scene = (PackedScene)ResourceLoader.Load(scenePath);
        sceneTree.CallDeferred("change_scene_to_packed", scene);
    }
}

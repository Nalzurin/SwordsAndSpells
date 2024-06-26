using Godot;
using System;

public partial class ButtonDamageTest : Button
{
    [Export]
    PlayerController player;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Pressed += OnPress;
    }
    public void OnPress()
    {
        player.PlayerData.Effects.AddEffect(new ChangeHealthEffect("TestEffect", "Damage", "Magical damage", 0, -10, "Pure", true, "Test"));
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}

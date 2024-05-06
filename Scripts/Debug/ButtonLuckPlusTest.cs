using Godot;
using System;

public partial class ButtonLuckPlusTest : Button
{
	[Export]
	PlayerEntity player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Pressed += OnPress;
	}
	public void OnPress()
	{
		player.Effects.AddEffect(new ChangeLuckEffect("TestEffect", "Luck Fortification", "Luck Fortification", 0, 10, "Pure", false));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



}

using Godot;
using System;

public partial class ButtonManaMinusTest : Button
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
		player.Effects.AddEffect(new ChangeManaEffect("TestEffect", "Mana Drain", "Mana Draining", 0, -10, "Mana Drain", true));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



}

using Godot;
using System;

public partial class ButtonDexterityMinusTest : Button
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
		player.Effects.AddEffect(new ChangeDexterityEffect("TestEffect", "Dexterity Negation", "Dexterity Negation", 5, -10, "Pure", true));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



}

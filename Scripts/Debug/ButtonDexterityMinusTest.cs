using Godot;
using System;

public partial class ButtonDexterityMinusTest : Button
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
		player.PlayerData.Effects.AddEffect(new ChangeDexterityEffect("TestEffect", "Dexterity Negation", "Dexterity Negation", 5, -10, "Pure", true, "Test"));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



}

using Godot;
using System;

public partial class ButtonIntelligencePlusTest : Button
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
		player.PlayerData.Effects.AddEffect(new ChangeIntelligenceEffect("TestEffect", "Intelligence Fortification", "Intelligence Fortification", 0, 10, "Pure", false, "Test"));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



}

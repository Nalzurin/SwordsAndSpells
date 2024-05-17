using Godot;
using System;

public partial class WorldBeginScript : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private GameManager gameManager;
	public override void _Ready()
	{
		gameManager = (GameManager)GetNode("/root/GameManager");
		gameManager.BeginTravel();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

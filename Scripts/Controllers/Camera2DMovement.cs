using Godot;
using System;

public partial class Camera2DMovement : Camera2D
{
    // Define the speed of the camera movement
    [Export] public float speed = 2000.0f;
    [Export] GameManager gameManager;
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        this.LimitBottom = gameManager.CurrentWorld.Settings.Size*16;
        this.LimitRight = gameManager.CurrentWorld.Settings.Size*16;
    }
}

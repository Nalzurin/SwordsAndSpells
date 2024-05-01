using Godot;
using System;

public partial class Camera2DMovement : Node2D
{
    // Define the speed of the camera movement
    [Export] public float speed = 200.0f;

    public override void _Process(double delta)
    {
        // Create a Vector2 to store the camera's movement direction
        Vector2 direction = Vector2.Zero;

        // Check for user input and adjust the direction vector accordingly
        if (Input.IsActionPressed("ui_up"))
        {
            direction.Y -= 1;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            direction.Y += 1;
        }
        if (Input.IsActionPressed("ui_left"))
        {
            direction.X -= 1;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            direction.X += 1;
        }

        // Normalize the direction vector to maintain consistent speed
        if (direction != Vector2.Zero)
        {
            direction = direction.Normalized();
        }

        // Calculate the movement vector
        Vector2 movement = direction * speed * (float)delta;

        // Update the camera's position
        Position += movement;
    }
}

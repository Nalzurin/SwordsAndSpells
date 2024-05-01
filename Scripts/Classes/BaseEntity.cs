using Godot;
using System;

public abstract partial class BaseEntity : Node, IEntity
{
    // IEntity properties
    public abstract string ID { get; set; }
    public abstract string AssetName { get; set; }

    public abstract string Description { get; set; }
    public abstract string Type { get; }
    public abstract string TypeDescription { get; }
    public abstract int Health { get; set; }
    public abstract string SpritePath { get; set; } 

    // IEntity methods
    public abstract void Instantiate(int x, int y);

    public virtual void TakeDamage(int value)
    {
        GD.Print($"{Name} took {value} damage!");
        Health -= value;
    }

    public virtual void Heal(int value)
    {
        GD.Print($"{Name} healed by {value}!");
        Health += value;
    }

    public virtual void Defeat()
    {
        GD.Print($"{Name} defeated!");
        QueueFree(); // Remove the entity from the scene
    }
}
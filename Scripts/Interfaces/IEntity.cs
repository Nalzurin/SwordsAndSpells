using Godot;
using System;

public interface IEntity : IAsset
{
    string Type { get; }
    string TypeDescription { get; }
    int Health { get; set; }
    Sprite2D EntitySprite { get; } 
    void Instantiate(int x, int y);
    void TakeDamage(int value);
    void Heal(int value);
    void Defeat();

}

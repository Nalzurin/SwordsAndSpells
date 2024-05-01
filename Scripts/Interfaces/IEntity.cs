using Godot;
using System;

public interface IEntity : IAsset
{
    string Type { get; }
    string TypeDescription { get; }
    void Instantiate(int x, int y);
    void TakeDamage(int value);
    void Heal(int value);
    void Defeat();

}

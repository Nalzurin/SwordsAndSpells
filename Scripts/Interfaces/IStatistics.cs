using Godot;
using System;

public interface IStatistics
{
    IClass CharacterClass { get; }   

    int health { get; }
    int mana { get; }
    void RestoreHealth(int amount);
    void RestoreMana(int amount);
    void RemoveHealth(int amount);
    void RemoveMana(int amount);
    

}

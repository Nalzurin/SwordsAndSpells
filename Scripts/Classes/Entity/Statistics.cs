using Godot;
using System;

public partial class Statistics : Resource
{
    public string CharacterID { get; set; }
    public int StepsWalked { get; private set; }
    public int DamageDealt { get; private set; }
    public int DamageTaken { get; private set; }
    public int HealthHealed { get; private set; }
    public int EnemiesDefeated { get; private set; }

    public Statistics() : this("DummyID", 0, 0, 0, 0, 0) { }
    public Statistics(string _CharacterID, int _StepsWalked, int _DamageDealt, int _DamageTaken, int _HealthHealed, int _EnemiesDefeated)
    {
        CharacterID = _CharacterID;
        StepsWalked = _StepsWalked;
        DamageDealt = _DamageDealt;
        DamageTaken = _DamageTaken;
        HealthHealed = _HealthHealed;
        EnemiesDefeated = _EnemiesDefeated;
    }


    // Method to update steps walked
    public void UpdateStepsWalked(int steps)
    {
        StepsWalked += steps;
    }

    // Method to update damage dealt
    public void UpdateDamageDealt(int damage)
    {
        DamageDealt += damage;
    }

    // Method to update damage taken
    public void UpdateDamageTaken(int damage)
    {
        DamageTaken += damage;
    }

    // Method to update health healed
    public void UpdateHealthHealed(int healed)
    {
        HealthHealed += healed;
    }

    // Method to update enemies defeated
    public void UpdateEnemiesDefeated(int count)
    {
        EnemiesDefeated += count;
    }

    // Method to reset all statistics
    public void ResetStatistics()
    {
        StepsWalked = 0;
        DamageDealt = 0;
        DamageTaken = 0;
        HealthHealed = 0;
        EnemiesDefeated = 0;
    }

    // Method to display statistics
    public void DisplayStatistics()
    {
        GD.Print($"Statistics for {CharacterID}:");
        GD.Print($"Steps Walked: {StepsWalked}");
        GD.Print($"Damage Dealt: {DamageDealt}");
        GD.Print($"Damage Taken: {DamageTaken}");
        GD.Print($"Health Healed: {HealthHealed}");
        GD.Print($"Enemies Defeated: {EnemiesDefeated}");
    }
}

using Godot;
using System;

public partial class DebugValuesScript : Node
{
    [Export]
    PlayerController player;
    [Export]
    Label HealthBase;
    [Export]
    Label HealthMax;
    [Export]
    Label HealthCurrent;
    [Export]
    Label StaminaBase;
    [Export]
    Label StaminaMax;
    [Export]
    Label StaminaCurrent;
    [Export]
    Label ManaBase;
    [Export]
    Label ManaMax;
    [Export]
    Label ManaCurrent;
    [Export]
    Label StrengthBase;
    [Export]
    Label StrengthCurrent;
    [Export]
    Label DexterityBase;
    [Export]
    Label DexterityCurrent;
    [Export]
    Label VitalityBase;
    [Export]
    Label VitalityCurrent;
    [Export]
    Label IntelligenceBase;
    [Export]
    Label IntelligenceCurrent;
    [Export]
    Label LuckBase;
    [Export]
    Label LuckCurrent;
    [Export]
    Label MeleeDamageMult;
    [Export]
    Label RangedDamageMult;
    [Export]
    Label MagicDamageMult;
    [Export]
    Label LuckMult;
    [Export]
    Label DodgeChance;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        CallDeferred("SetSignal");
        CallDeferred("UpdateValues", "Player");
    }
    public void SetSignal()
    {
        player.PlayerData.Characteristics.CharacteristicsChanged += UpdateValues;
    }
    public void UpdateValues(string source)
    {
        if (source == "Player")
        {
            HealthBase.Text = player.PlayerData.Characteristics.HealthBase.ToString();
            HealthMax.Text = player.PlayerData.Characteristics.HealthMax.ToString();
            HealthCurrent.Text = player.PlayerData.Characteristics.HealthCurrent.ToString();
            StaminaBase.Text = player.PlayerData.Characteristics.StaminaBase.ToString();
            StaminaMax.Text = player.PlayerData.Characteristics.StaminaMax.ToString();
            StaminaCurrent.Text = player.PlayerData.Characteristics.StaminaCurrent.ToString();
            ManaBase.Text = player.PlayerData.Characteristics.ManaBase.ToString();
            ManaMax.Text = player.PlayerData.Characteristics.ManaMax.ToString();
            ManaCurrent.Text = player.PlayerData.Characteristics.ManaCurrent.ToString();
            StrengthBase.Text = player.PlayerData.Characteristics.StrengthBase.ToString();
            StrengthCurrent.Text = player.PlayerData.Characteristics.StrengthCurrent.ToString();
            DexterityBase.Text = player.PlayerData.Characteristics.DexterityBase.ToString();
            DexterityCurrent.Text = player.PlayerData.Characteristics.DexterityCurrent.ToString();
            VitalityBase.Text = player.PlayerData.Characteristics.VitalityBase.ToString();
            VitalityCurrent.Text = player.PlayerData.Characteristics.VitalityCurrent.ToString();
            IntelligenceBase.Text = player.PlayerData.Characteristics.IntelligenceBase.ToString();
            IntelligenceCurrent.Text = player.PlayerData.Characteristics.IntelligenceCurrent.ToString();
            LuckBase.Text = player.PlayerData.Characteristics.LuckBase.ToString();
            LuckCurrent.Text = player.PlayerData.Characteristics.LuckCurrent.ToString();
            MeleeDamageMult.Text = player.PlayerData.Characteristics.MeleeDamageMult.ToString();
            RangedDamageMult.Text = player.PlayerData.Characteristics.RangedDamageMult.ToString();
            MagicDamageMult.Text = player.PlayerData.Characteristics.MagicDamageMult.ToString();
            LuckMult.Text = player.PlayerData.Characteristics.LuckMult.ToString();
            DodgeChance.Text = player.PlayerData.Characteristics.DodgeChance.ToString();

        }


    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

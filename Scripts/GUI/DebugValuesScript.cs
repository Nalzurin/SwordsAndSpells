using Godot;
using System;

public partial class DebugValuesScript : Node
{
    [Export]
    PlayerEntity player;
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

        player.Characteristics.CharacteristicsChanged += UpdateValues;
        CallDeferred("UpdateValues", "Player");
    }
    public void UpdateValues(string source)
    {
        if (source == "Player")
        {
            HealthBase.Text = player.Characteristics.HealthBase.ToString();
            HealthMax.Text = player.Characteristics.HealthMax.ToString();
            HealthCurrent.Text = player.Characteristics.HealthCurrent.ToString();
            StaminaBase.Text = player.Characteristics.StaminaBase.ToString();
            StaminaMax.Text = player.Characteristics.StaminaMax.ToString();
            StaminaCurrent.Text = player.Characteristics.StaminaCurrent.ToString();
            ManaBase.Text = player.Characteristics.ManaBase.ToString();
            ManaMax.Text = player.Characteristics.ManaMax.ToString();
            ManaCurrent.Text = player.Characteristics.ManaCurrent.ToString();
            StrengthBase.Text = player.Characteristics.StrengthBase.ToString();
            StrengthCurrent.Text = player.Characteristics.StrengthCurrent.ToString();
            DexterityBase.Text = player.Characteristics.DexterityBase.ToString();
            DexterityCurrent.Text = player.Characteristics.DexterityCurrent.ToString();
            VitalityBase.Text = player.Characteristics.VitalityBase.ToString();
            VitalityCurrent.Text = player.Characteristics.VitalityCurrent.ToString();
            IntelligenceBase.Text = player.Characteristics.IntelligenceBase.ToString();
            IntelligenceCurrent.Text = player.Characteristics.IntelligenceCurrent.ToString();
            LuckBase.Text = player.Characteristics.LuckBase.ToString();
            LuckCurrent.Text = player.Characteristics.LuckCurrent.ToString();
            MeleeDamageMult.Text = player.Characteristics.MeleeDamageMult.ToString();
            RangedDamageMult.Text = player.Characteristics.RangedDamageMult.ToString();
            MagicDamageMult.Text = player.Characteristics.MagicDamageMult.ToString();
            LuckMult.Text = player.Characteristics.LuckMult.ToString();
            DodgeChance.Text = player.Characteristics.DodgeChance.ToString();

        }


    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

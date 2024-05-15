using Godot;
using System;
using System.Collections.Generic;

public partial class Characteristics : Node
{
    [Signal]
    public delegate void CharacteristicsChangedEventHandler(string Entity);
    [Signal]
    public delegate void HealthChangedEventHandler();
    [Signal]
    public delegate void StaminaChangedEventHandler();
    [Signal]
    public delegate void ManaChangedEventHandler();
    [Signal]
    public delegate void AttributesChangedEventHandler();

    public struct AttributeModifier
    {
        public string source;
        public int value;
        public AttributeModifier(string _source, int _value)
        {
            source = _source;
            value = _value;
        }
    }
    private BaseEntity parent { get; set; }
    #region AttributesVariables
    public int CharacterLevel { get; set; }
    public int HealthBase { get; private set; }
    public int HealthMax { get; private set; }
    public int HealthCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> HealthModifiers = new Dictionary<int, AttributeModifier>();
    public int StaminaBase { get; private set; }
    public int StaminaMax { get; private set; }
    public int StaminaCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> StaminaModifiers = new Dictionary<int, AttributeModifier>();
    public int ManaBase { get; private set; }
    public int ManaMax { get; private set; }
    public int ManaCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> ManaModifiers = new Dictionary<int, AttributeModifier>();
    public int StrengthBase { get; private set; }
    public int StrengthCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> StrengthModifiers = new Dictionary<int, AttributeModifier>();
    public int DexterityBase { get; private set; }
    public int DexterityCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> DexterityModifiers = new Dictionary<int, AttributeModifier>();
    public int VitalityBase { get; private set; }
    public int VitalityCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> VitalityModifiers = new Dictionary<int, AttributeModifier>();
    public int IntelligenceBase { get; private set; }
    public int IntelligenceCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> IntelligenceModifiers = new Dictionary<int, AttributeModifier>();
    public int LuckBase { get; private set; }
    public int LuckCurrent { get; private set; }
    public Dictionary<int, AttributeModifier> LuckModifiers = new Dictionary<int, AttributeModifier>();
    #endregion
    #region StatVariables
    public float MeleeDamageMult { get; private set; }
    public float RangedDamageMult { get; private set; }
    public float MagicDamageMult { get; private set; }
    public float LuckMult { get; private set; }
    public float DodgeChance { get; private set; }
    public Dictionary<int, AttributeModifier> DodgeChanceModifiers = new Dictionary<int, AttributeModifier>();

    #endregion
    #region Value Functions
    public void ChangeHealth(int amount)
    {
        HealthCurrent += amount;
        if (HealthCurrent > HealthMax)
        {
            HealthCurrent = HealthMax; // Clamp to max
        }
        if (HealthCurrent < 0)
        {
            HealthCurrent = 0; // Clamp to 0
        }
        EmitSignal(SignalName.HealthChanged);
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void ChangeStamina(int amount)
    {
        StaminaCurrent += amount;
        if (StaminaCurrent > StaminaMax)
        {
            StaminaCurrent = StaminaMax; // Clamp to max
        }
        if (StaminaCurrent < 0)
        {
            StaminaCurrent = 0; // Clamp to 0
        }
        EmitSignal(SignalName.StaminaChanged);
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void ChangeMana(int amount)
    {
        ManaCurrent += amount;
        if (ManaCurrent > ManaMax)
        {
            ManaCurrent = ManaMax; // Clamp to max
        }
        if (ManaCurrent < 0)
        {
            ManaCurrent = 0; // Clamp to 0
        }
        EmitSignal(SignalName.ManaChanged);
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void IncreaseLevel()
    {
        CharacterLevel++;
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void IncreaseStrength()
    {
        StrengthBase++;
        StrengthCurrent++;
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void IncreaseDexterity()
    {
        DexterityBase++;
        DexterityCurrent++;
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void IncreaseVitality()
    {
        VitalityBase++;
        VitalityCurrent++;
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void IncreaseIntelligence()
    {
        IntelligenceBase++;
        IntelligenceCurrent++;
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");

    }
    public void IncreaseLuck()
    {
        LuckBase++;
        LuckCurrent++;
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    #endregion
    public int AddModifier(ref Dictionary<int, AttributeModifier> modifiers, string source, int value)
    {
        int index = 0;
        while (modifiers.ContainsKey(index))
        {
            index++;
        }
        modifiers[index] = new AttributeModifier(source, value);
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
        return index;
        
    }
    public void RemoveModifier(ref Dictionary<int, AttributeModifier> modifiers, int id)
    {
        if (modifiers.ContainsKey(id))
        {
            if (modifiers == HealthModifiers)
            {
                HealthCurrent -= modifiers[id].value;
            }
            if (modifiers == StaminaModifiers)
            {
                StaminaCurrent -= modifiers[id].value;
            }
            if (modifiers == ManaModifiers)
            {
                ManaCurrent -= modifiers[id].value;
            }
            modifiers.Remove(id);
        }
        CalculateModifiers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");

    }

    public void CalculateModifiers()
    {

        //strength
        StrengthCurrent = StrengthBase;
        foreach (AttributeModifier modifier in StrengthModifiers.Values)
        {
            StrengthCurrent += modifier.value;
            if(StrengthCurrent < 0)
            {
                StrengthCurrent = 0;
            }
        }
        //dexterity
        DexterityCurrent = DexterityBase;
        foreach (AttributeModifier modifier in DexterityModifiers.Values)
        {
            DexterityCurrent += modifier.value;
            if (DexterityCurrent < 0)
            {
                DexterityCurrent = 0;
            }
        }
        //vitality
        VitalityCurrent = VitalityBase;
        foreach (AttributeModifier modifier in VitalityModifiers.Values)
        {
            VitalityCurrent += modifier.value;
            if (VitalityCurrent < 0)
            {
                VitalityCurrent = 0;
            }
        }
        //intelligence
        IntelligenceCurrent = IntelligenceBase;
        foreach (AttributeModifier modifier in IntelligenceModifiers.Values)
        {
            IntelligenceCurrent += modifier.value;
            if (IntelligenceCurrent < 0)
            {
                IntelligenceCurrent = 0;
            }
        }
        //luck
        LuckCurrent = LuckBase;
        foreach (AttributeModifier modifier in LuckModifiers.Values)
        {
            LuckCurrent += modifier.value;
            if (LuckCurrent < 0)
            {
                LuckCurrent = 0;
            }
        }
        //health
        int healthBasePrevious = HealthBase;
        if (parent is PlayerEntity)
        {
            HealthBase = 50 + VitalityCurrent / 2;
        }
        HealthMax = HealthBase;
        HealthCurrent = HealthCurrent - healthBasePrevious + HealthBase;
        foreach (AttributeModifier modifier in HealthModifiers.Values)
        {
            HealthMax += modifier.value;
            HealthCurrent += modifier.value;
        }
        EmitSignal(SignalName.HealthChanged);
        //stamina
        int staminaBasePrevious = StaminaBase;
        if (parent is PlayerEntity)
        {
            StaminaBase = 50 + VitalityCurrent / 4 + StrengthCurrent / 4 + DexterityCurrent / 4 + IntelligenceCurrent / 4;
        }
        StaminaMax = StaminaBase;
        StaminaCurrent = StaminaCurrent - staminaBasePrevious + StaminaBase;
        foreach (AttributeModifier modifier in StaminaModifiers.Values)
        {
            StaminaMax += modifier.value;
            StaminaCurrent += modifier.value;
        }
        EmitSignal(SignalName.StaminaChanged);
        //mana
        int manaBasePrevious = ManaBase;
        if (parent is PlayerEntity)
        {
            ManaBase = 50 + IntelligenceCurrent / 2;
        }
        ManaMax = ManaBase;
        ManaCurrent = ManaCurrent - manaBasePrevious + ManaBase;
        foreach (AttributeModifier modifier in ManaModifiers.Values)
        {
            ManaMax += modifier.value;
            ManaCurrent += modifier.value;
        }
        EmitSignal(SignalName.ManaChanged);
        CalculateMultipliers();
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }
    public void CalculateMultipliers()
    {
        MeleeDamageMult = (StrengthCurrent + 50f) / 100f;
        RangedDamageMult = (DexterityCurrent + 50f) / 100f;
        MagicDamageMult = 0.5f + (IntelligenceCurrent + 50f) / 100f;
        LuckMult = 1f + (LuckCurrent / 100f);
        DodgeChance = 0.05f + (DexterityCurrent / 100f) * 0.5f + (LuckCurrent / 100f) * 0.3f;
        foreach(AttributeModifier modifier in DodgeChanceModifiers.Values)
        {
            DodgeChance += modifier.value;
        }
        EmitSignal(SignalName.AttributesChanged);
        EmitSignal(SignalName.CharacteristicsChanged, "Player");
    }

    public void SetParent(BaseEntity _parent)
    {
        parent = _parent;
    }
    public Characteristics() : this(1, 50, 50, 50, 50, 50, 50, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,null) { }

    public Characteristics(int _CharacterLevel, int healthBase, int healthCurrent, int staminaBase, int staminaCurrent, int manaBase, int manaCurrent, int strengthBase, int strengthCurrent,
                           int dexterityBase, int dexterityCurrent, int vitalityBase, int vitalityCurrent, int intelligenceBase, int intelligenceCurrent, int luckBase,
                           int luckCurrent, BaseEntity _parent)
    {
        CharacterLevel = _CharacterLevel;
        HealthBase = healthBase;
        HealthCurrent = healthCurrent;
        StaminaBase = staminaBase;
        StaminaCurrent = staminaCurrent;
        ManaBase = manaBase;
        ManaCurrent = manaCurrent;
        StrengthBase = strengthBase;
        StrengthCurrent = strengthCurrent;
        DexterityBase = dexterityBase;
        DexterityCurrent = dexterityCurrent;
        VitalityBase = vitalityBase;
        VitalityCurrent = vitalityCurrent;
        IntelligenceBase = intelligenceBase;
        IntelligenceCurrent = intelligenceCurrent;
        LuckBase = luckBase;
        LuckCurrent = luckCurrent;
        CalculateModifiers();
        parent = _parent;

    }
}


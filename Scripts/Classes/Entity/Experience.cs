using Godot;
using System;

public class Experience 
{
    private BaseEntity parent {  get; set; }

    public int CharacterExp { get; set; }
    public int StrengthExp { get; set; }
    public int DexterityExp {  get; set; }
    public int VitalityExp {  get; set; }
    public int IntelligenceExp {  get; set; }
    public int LuckExp {  get; set; }
    public void AddCharacterExp(int value)
    {
        CharacterExp += value;
        LevelUpCharacter();
        
    }
    private void LevelUpCharacter()
    {   
        while(ExpFormula(CharacterExp, parent.Characteristics.CharacterLevel))
        {
            parent.Characteristics.IncreaseLevel();
        }
    }
    public void AddStrengthExp(int value)
    {
        StrengthExp += value;
        LevelUpStrength();

    }
    private void LevelUpStrength()
    {
        while (ExpFormula(StrengthExp, parent.Characteristics.StrengthBase))
        {
            parent.Characteristics.IncreaseStrength();
        }
    }
    public void AddDexterityExp(int value)
    {
        DexterityExp += value;
        LevelUpDexterity();

    }
    private void LevelUpDexterity()
    {
        while (ExpFormula(DexterityExp, parent.Characteristics.DexterityBase))
        {
            parent.Characteristics.IncreaseDexterity();
        }
    }
    public void AddVitalityExp(int value)
    {
        VitalityExp += value;
        LevelUpVitality();

    }
    private void LevelUpVitality()
    {
        while (ExpFormula(VitalityExp, parent.Characteristics.VitalityBase))
        {
            parent.Characteristics.IncreaseVitality();
        }
    }
    public void AddIntelligenceExp(int value)
    {
        IntelligenceExp += value;
        LevelUpIntelligence();

    }
    private void LevelUpIntelligence()
    {
        while (ExpFormula(IntelligenceExp, parent.Characteristics.IntelligenceBase))
        {
            parent.Characteristics.IncreaseIntelligence();
        }
    }
    public void AddLuckExp(int value)
    {
        LuckExp += value;
        LevelUpLuck();

    }
    private void LevelUpLuck()
    {
        while (ExpFormula(LuckExp, parent.Characteristics.LuckBase))
        {
            parent.Characteristics.IncreaseLuck();
        }
    }

    private bool ExpFormula(int value, int level)
    {
        return value >= 50 + level * 100;
    }

    public Experience() : this(null, 0, 0, 0, 0, 0, 0) { }
    public Experience(BaseEntity _parent, int _CharacterExp, int _StrengthExp, int _DexterityExp, int _VitalityExp, int _IntelligenceExp, int _LuckExp)
    {
        parent = _parent;
        CharacterExp = _CharacterExp;
        StrengthExp = _StrengthExp;
        DexterityExp = _DexterityExp;
        VitalityExp = _VitalityExp;
        IntelligenceExp = _IntelligenceExp;
        LuckExp = _LuckExp;

    }


}

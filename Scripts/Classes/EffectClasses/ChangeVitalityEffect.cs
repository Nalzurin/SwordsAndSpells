using Godot;
using Godot.Collections;
using System;

public class ChangeVitalityEffect : IEffect
{
    public string ID { get; }
    public string EffectName { get; }
    public string Description { get; }
    public int Duration { get; private set; }
    public int Value { get; }
    public string EffectType { get; } // if applicable
    public string Source { get; }
    public bool IsHarmful {  get; private set; }

    private int modifierIndex;
    public void Apply(ITargetable target)
    {
       modifierIndex = target.Characteristics.AddModifier(ref target.Characteristics.VitalityModifiers, Source, Value);
    }
    public bool Update(ITargetable target)
    {
        if(Duration == -1)
        {
            Apply(target);
            return true;
        }
        if (Duration > 0)
        {
            Duration--;
            return true;
        }
        else
        {
            target.Characteristics.RemoveModifier(ref target.Characteristics.VitalityModifiers, modifierIndex);
            return false;
        }

    }
    public ChangeVitalityEffect() : this("DummyVitalityChangeEffect", "Dummy Vitality Change Effect", "This is a dummy Vitality change effect, if you see this, it means something went wrong", 0, 0, "Pure", false) { }
    public ChangeVitalityEffect(string _ID, string _EffectName, string _Description, int _Duration, int _Value, string _EffectType, bool _IsHarmful)
    {
        ID = _ID;
        EffectName = _EffectName;
        Description = _Description;
        Duration = _Duration;
        Value = _Value;
        EffectType = _EffectType;
        IsHarmful = _IsHarmful;
    }

}
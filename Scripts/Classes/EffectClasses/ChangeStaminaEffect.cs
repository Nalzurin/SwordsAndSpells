using Godot;
using Godot.Collections;
using System;

public class ChangeStaminaEffect : IEffect
{
    public string ID { get; }
    public string EffectName { get; }
    public string Description { get; }
    public int Duration { get; private set; }
    public int Value { get; }
    public string EffectType { get; } // if applicable
    public bool IsHarmful { get; private set; }
    public void Apply(ITargetable target)
    {
        target.Characteristics.ChangeStamina(Value);
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
            Apply(target);
            return true;
        }
        else
        {
            return false;
        }

    }
    public ChangeStaminaEffect() : this("DummyManaChangeEffect", "Dummy Mana Change Effect", "This is a dummy mana change effect, if you see this, it means something went wrong", 0, 0, "Pure", false) { }
    public ChangeStaminaEffect(string _ID, string _EffectName, string _Description, int _Duration, int _Value, string _EffectType, bool isHarmful)
    {
        ID = _ID;
        EffectName = _EffectName;
        Description = _Description;
        Duration = _Duration;
        Value = _Value;
        EffectType = _EffectType;
        IsHarmful = isHarmful;
    }

}
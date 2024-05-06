using Godot;
using System;
using System.Collections.Generic;

public class AbilityPassive : BaseAbility
{
    public List<IEffect> Effects { get; private set; }
    public IEffect GetEffect(int index)
    {
        return Effects[index];
    }

    public AbilityPassive(): this("DummyPassiveAbilityID", "Dummy Ability Passive", "If you see this ability, something went wrong", "Assets/Sprites/Items/Consumables/Food/Lemon.png", new List<string> { "Vitality"}, new List<IEffect> { new ChangeHealthEffect()}) { }
    public AbilityPassive(string _ID, string _AbilityName, string _AbilityDescription,  string _SpritePath, List<string> _Attributes, List<IEffect> _Effects) 
    {
        ID = _ID;
        AbilityName = _AbilityName;
        Description = _AbilityDescription;
        SpritePath = _SpritePath;
        Attributes = _Attributes;
        Effects = _Effects;
    }



}

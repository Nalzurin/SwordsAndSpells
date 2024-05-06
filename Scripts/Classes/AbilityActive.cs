using Godot;
using System;
using System.Collections.Generic;

public class AbilityActive : BaseAbility, IHasActions
{
    public List<BaseAction> Actions { get; private set; }
    public List<BaseAction> GetActions() { return Actions; }
    public BaseAction GetAction(int index)
    {
        return Actions[index];
    }

    public AbilityActive(): this("DummyActiveAbilityID", "Dummy Ability Active", "If you see this ability, something went wrong", "Assets/Sprites/Items/Consumables/Food/Lemon.png", new List<string> { "Vitality"}, new List<BaseAction> { new BaseAction()}) { }
    public AbilityActive(string _ID, string _AbilityName, string _AbilityDescription,  string _SpritePath, List<string> _Attributes, List<BaseAction> _Actions) 
    {
        ID = _ID;
        AbilityName = _AbilityName;
        Description = _AbilityDescription;
        SpritePath = _SpritePath;
        Attributes = _Attributes;
        Actions = _Actions;
    }



}

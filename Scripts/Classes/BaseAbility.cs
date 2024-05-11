using Godot;
using System;
using System.Collections.Generic;

public class BaseAbility : IHasActions
{
    public string ID { get; set; }
    public string AbilityName { get; set; }
    public string Description { get; set; }
    public string SpritePath { get; set; }
    public bool IsActive { get; set; }
    public Dictionary<string, int> Prerequisites { get; set; }
    public List<string> Attributes { get; set; }
    public List<IEffect> Effects { get; set; }
    public IEffect GetEffect(int index)
    {
        return Effects[index];
    }
    public List<BaseAction> Actions { get; set; }
    public List<BaseAction> GetActions() { return Actions; }
    public BaseAction GetAction(int index)
    {
        return Actions[index];
    }

    public BaseAbility() : this("DummyPassiveAbilityID", "Dummy Ability Passive", "If you see this ability, something went wrong", "Assets/Sprites/Items/Consumables/Food/Lemon.png", new List<string> { "Vitality" }, true, new Dictionary<string, int> { {"Strength", 25 } }, new List<IEffect> { new ChangeHealthEffect() }, new List<BaseAction> { new BaseAction()}) { }
    public BaseAbility(string _ID, string _AbilityName, string _AbilityDescription, string _SpritePath, List<string> _Attributes, bool _IsActive, Dictionary<string,int> _Prerequisites, List<IEffect> _Effects, List<BaseAction> _Actions)
    {
        ID = _ID;
        AbilityName = _AbilityName;
        Description = _AbilityDescription;
        SpritePath = _SpritePath;
        Attributes = _Attributes;
        IsActive = _IsActive;
        Prerequisites = _Prerequisites;
        Effects = _Effects;
        Actions = _Actions;
    }

}

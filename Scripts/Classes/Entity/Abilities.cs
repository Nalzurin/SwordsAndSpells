using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

public class Abilities
{
    private BaseEntity parent {  get; set; }
    public List<BaseAbility> abilities { get; private set; }

    public void SetParent(BaseEntity parent)
    {
        this.parent = parent;
    }
    public void ApplyAbilities()
    {
        foreach(BaseAbility ability in abilities)
        {
            foreach (BaseAction action in ability.Actions)
            {
                if (!parent.Actions.actions.Contains(action)) parent.Actions.AddAction(action);
            }
            foreach (IEffect effect in ability.Effects)
            {
                if (!parent.Effects.GetActiveEffects().Contains(effect)) parent.Effects.AddEffect(effect);
            }


        }
    }
    public void AddAbility(BaseAbility ability)
    {
        abilities.Add(ability);
        foreach (BaseAction action in ability.Actions)
        {
            parent.Actions.AddAction(action);
        }
        foreach (IEffect effect in ability.Effects)
        {
            parent.Effects.AddEffect(effect);
        }
        
    }
    public void RemoveAbility(BaseAbility ability)
    {
        abilities.Remove(ability);
        foreach (BaseAction action in ability.Actions)
        {
            parent.Actions.RemoveAction(action);
        }
        foreach (IEffect effect in ability.Effects)
        {
            parent.Effects.RemoveEffect(effect);
        }

    }
    public Abilities() : this(new List<BaseAbility>(), null) { }
    public Abilities(List<BaseAbility> _Abilities, BaseEntity _Parent)
    {
        abilities = _Abilities;
        parent = _Parent;
    }
}

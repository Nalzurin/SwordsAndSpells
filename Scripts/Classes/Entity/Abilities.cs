using Godot;
using System;
using System.Collections.Generic;

public class Abilities
{
    private BaseEntity parent {  get; set; }
    public List<BaseAbility> abilities { get; private set; }

    public void SetParent(BaseEntity parent)
    {
        this.parent = parent;
    }
    public void AddAbility(BaseAbility ability)
    {
        abilities.Add(ability);
        if(ability is AbilityActive abilityActive)
        {
            foreach (BaseAction action in abilityActive.Actions) 
            {
                parent.Actions.AddAction(action);
            }
        }
       if(ability is AbilityPassive abilityPassive)
        {
            foreach (IEffect effect in abilityPassive.Effects)
            {
                parent.Effects.AddEffect(effect);
            }
        }
    }
    public void RemoveAbility(BaseAbility ability)
    {
        abilities.Remove(ability);
        if (ability is AbilityActive abilityActive)
        {

            foreach (BaseAction action in abilityActive.Actions)
            {
                parent.Actions.RemoveAction(action);
            }
        }
        if (ability is AbilityPassive abilityPassive)
        {
            foreach (IEffect effect in abilityPassive.Effects)
            {
                parent.Effects.RemoveEffect(effect);
            }
        }
    }
}

using Godot;
using System;
using System.Collections.Generic;

public class Effects
{
    private BaseEntity parent;
    private List<IEffect> activeEffects = new List<IEffect>();
    public void AddEffect(IEffect effect)
    {
        effect.Apply(parent);
        if (effect.Duration > 0)
        {
            activeEffects.Add(effect);
        }
        
        
    }
    public void UpdateEffects()
    {
        activeEffects.RemoveAll(effect => !effect.Update(parent));
    }
    public void RemoveEffect(IEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
        }
    }
    public void SetParent(BaseEntity _parent)
    {
        parent = _parent;
    }
    public Effects() : this(null) { }
    public Effects(BaseEntity _parent)
    {
        parent = _parent;

    }
}

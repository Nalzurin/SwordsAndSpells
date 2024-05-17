using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public interface IEffect
{
    public string ID { get;}
    public string EffectName { get;}
    public string Description { get;}
    public int Duration { get;}
    public int Value { get; }
    public bool IsHarmful { get;}
    void Apply(ITargetable target);
    bool Update(ITargetable target);
}

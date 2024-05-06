using Godot;
using System;
using System.Collections.Generic;

public abstract class BaseAbility
{
    public string ID { get; set; }
    public string AbilityName { get; set; }
    public string Description { get; set; }
    public string SpritePath { get; set; }
    public List<string> Attributes { get; set; }


}

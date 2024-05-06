using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public interface ITargetable
{
    public string TargetType { get; set; }
    Characteristics Characteristics { get; set; }
    Effects Effects { get; set; }
}

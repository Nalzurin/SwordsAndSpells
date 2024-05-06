using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public interface IHasActions
{
    List<BaseAction> Actions { get; }
    public List<BaseAction> GetActions();

}

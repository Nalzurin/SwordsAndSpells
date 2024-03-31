using Godot;
using System;
using System.Collections.Generic;

public interface IItem
{
    string Item_Name { get; }
    string Description { get; }
    string Type { get; }
    string TypeDescription { get; }
    int value { get; }
    List<IAction> Actions { get; }

}

using Godot;
using System;

public interface IAction
{
    string Name { get; }
    string Description { get; }
    string Type { get; }
    void Do_Action();
}

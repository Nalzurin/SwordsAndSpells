using Godot;
using System;

public interface IClass
{
    string Name { get; }
    string Description { get; }
    string Type { get; }
    void Do_Action();
}

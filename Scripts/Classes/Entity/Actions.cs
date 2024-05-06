using Godot;
using System;
using System.Collections.Generic;

public class Actions
{
    private BaseEntity parent {  get; set; }
    public List<BaseAction> actions { get; private set; }
    public void AddAction(BaseAction action)
    {
        actions.Add(action);
    }
    public void RemoveAction(BaseAction action)
    {
        actions.Remove(action);
    }
    public void RemoveAction(int  index)
    {
        actions.RemoveAt(index);
    }
}

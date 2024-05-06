using Godot;
using System;
using System.Collections.Generic;

public partial class ItemConsumable : BaseItem, IHasActions
{
    public int UsesTotal { get; private set; }
    public int UsesLeft { get; private set; }
    public string Type { get; private set; }
    public string Target {  get; private set; }
    public List<BaseAction> Actions { get; private set; }
    public List<BaseAction> GetActions() { return Actions; }
    public BaseAction GetAction(int index)
    {
        return Actions[index];
    }


}
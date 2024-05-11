using Godot;
using System;
using System.Collections.Generic;

public partial class ItemConsumable : BaseItem, IHasActions
{
    public int UsesTotal { get; set; }
    public int UsesLeft { get; set; }
    public string Type { get; set; }
    public List<BaseAction> Actions { get; set; }
    public List<BaseAction> GetActions() { return Actions; }
    public BaseAction GetAction(int index)
    {
        return Actions[index];
    }
    public ItemConsumable():this("DummyItemConsumableID","Dummy Consumable", "If you see this text, something went wrong with the instancing","Legendary",0, "Assets/Sprites/Items/Consumables/Food/Lemon.png",1,1,"Bug", new List<BaseAction> { new BaseAction() }) { }
    public ItemConsumable(string iD, string itemName, string description, string rarity, int itemLevel, string spritePath, int usesTotal, int usesLeft, string type, List<BaseAction> actions)
    {
        ID = iD;
        ItemName = itemName;
        Description = description;
        Rarity = rarity;
        ItemLevel = itemLevel;
        SpritePath = spritePath;
        UsesTotal = usesTotal;
        UsesLeft = usesLeft;
        Type = type;

        Actions = actions;
    }
}
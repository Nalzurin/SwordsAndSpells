using Godot;
using System;
using System.Collections.Generic;

public partial class ItemGear : BaseItem, IHasActions
{
    public int GearSlot { get; set; }
    public List<IEffect> Effects { get; set; }
    public List<BaseAction> Actions { get; set; }
    public List<BaseAction> GetActions() { return Actions; }
    public BaseAction GetAction(int index)
    {
        return Actions[index];
    }
    public ItemGear() : this("DummyItemGearID", "Dummy Item Gear", "If you see this text, that means something went wrong with the item instancing", "Legendary", 0, "Assets/Sprites/Items/Consumables/Food/Lemon.png", 0, new List<IEffect> {}, new List<BaseAction> { new BaseAction() }) { }
    public ItemGear(string iD, string itemName, string description, string rarity, int itemLevel, string spritePath, int gearSlot, List<IEffect> effects, List<BaseAction> actions)
    {
        ID = iD;
        ItemName = itemName;
        Description = description;
        Rarity = rarity;
        ItemLevel = itemLevel;
        SpritePath = spritePath;
        GearSlot = gearSlot;
        Effects = effects;
        Actions = actions;
    }
}
using Godot;
using System;

public class Inventory
{
    public PlayerEntity parent {  get; set; }
    public BaseItem[] items { get; set; }
    public void SetParent(PlayerEntity _parent)
    {
        parent = _parent;
    }
    public void ApplyEffects()
    {
        for(int i = 0; i < 9; i++)
        {
            if(items[i] is ItemGear gear)
            {
                foreach(IEffect effect in gear.Effects)
                {
                    parent.Effects.AddEffect(effect);
                }
                
            }
        }
    }
    public void ApplyActions()
    {
        for (int i = 0; i < 9; i++)
        {
            if (items[i] is ItemGear gear)
            {
                foreach (BaseAction action in gear.Actions)
                {
                    parent.Actions.AddAction(action);
                }

            }
        }
    }
    public bool MoveItem(int from, int to)
    {
        if (items[from] == null)
        {
            return false;
        }
        if (items[to] == null)
        {
            if (to < 9)
            {
                if (items[from] is ItemGear gear)
                {
                    if (gear.GearSlot == to)
                    {
                        items[to] = items[from];
                        items[from] = null;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            items[to] = items[from];
            items[from] = null;
            return true;
        }
        if (items[from] != null && items[to] != null)
        {
            BaseItem item = items[from];
            items[from] = items[to];
            items[to] = item;
            return true;
        }
        return false;


    }
    public void DeleteItem(int index)
    {
        items[index] = null;
    }
    public bool AddItem(int index, BaseItem item)
    {
        if (items[index] == null)
        {
            items[index] = item;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool AddItem(BaseItem item)
    {
        int index = 9;
        while (index < items.Length)
        {
            if (items[index] == null)
            {
                items[index] = item;
                return true;
            }
        }
        return false;
    }
    public Inventory() : this(new BaseItem[39], null) { }
    public Inventory(BaseItem[] items, PlayerEntity parent)
    {
        this.items = items;
        this.parent = parent;
    }
}

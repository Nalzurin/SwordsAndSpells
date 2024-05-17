using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public partial class ItemLoader : Node
{
    const bool Export = true;
    private const string UserItemsFolder = "user://Assets/Items/";
    private const string ItemsFolder = "res://Assets/Items/";
    private AssetManager _assets;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _assets = (AssetManager)GetNode("/root/AssetManager");
        LoadItems();
        _assets.DebugListItems(); 
    }
    private void LoadItems()
    {
        string[] files;
        if (Export)
        {
            files = DirAccess.GetFilesAt(UserItemsFolder);
        }
        else
        {
            files = DirAccess.GetFilesAt(ItemsFolder);
        }
        foreach (string file in files)
        {
            if (file.EndsWith(".xml"))
            {
                LoadItem(file);
            }
            
        }
    }
    private void LoadItem(string filePath)
    {
        try
        {

            XmlDocument xmlDoc = new XmlDocument();
            if (Export)
            {
                xmlDoc.Load(ProjectSettings.GlobalizePath(UserItemsFolder + filePath));
            }
            else
            {
                xmlDoc.Load(ProjectSettings.GlobalizePath(ItemsFolder + filePath));
            }
            

            XmlNodeList itemNodes = xmlDoc.GetElementsByTagName("Item");
            foreach (XmlNode node in itemNodes)
            {
                BaseItem item = ProcessItem(node);
                _assets.AddItem(item);
            }
        }
        catch (Exception e)
        {
            GD.PrintErr("Error loading item from file: " + filePath);
            GD.PrintErr(e.Message);
        }
    }
    private BaseItem ProcessItem(XmlNode itemNode)
    {
        // Determine the item class
        XmlNode itemClassNode = itemNode.SelectSingleNode("ItemClass");
        string itemClass = itemClassNode?.InnerText;

        BaseItem item;
        if (itemClass == "ItemGear")
        {
            item = new ItemGear();
            ProcessItemGearNode(itemNode, (ItemGear)item);
        }
        else if (itemClass == "ItemConsumable")
        {
            item = new ItemConsumable();
            ProcessItemConsumableNode(itemNode, (ItemConsumable)item);
        }
        else
        {
            throw new Exception($"Unknown item class: {itemClass}");
        }

        // Process common item properties
        XmlNode idNode = itemNode.SelectSingleNode("ID");
        if (idNode != null)
            item.ID = idNode.InnerText;

        XmlNode nameNode = itemNode.SelectSingleNode("Name");
        if (nameNode != null)
            item.ItemName = nameNode.InnerText;

        XmlNode descNode = itemNode.SelectSingleNode("Description");
        if (descNode != null)
            item.Description = descNode.InnerText;

        XmlNode rarityNode = itemNode.SelectSingleNode("Rarity");
        if (rarityNode != null)
            item.Rarity = rarityNode.InnerText;

        XmlNode itemLevelNode = itemNode.SelectSingleNode("ItemLevel");
        if (itemLevelNode != null)
        {
            if (int.TryParse(itemLevelNode.InnerText, out int itemLevel))
                item.ItemLevel = itemLevel;
        }

        XmlNode spritePathNode = itemNode.SelectSingleNode("SpritePath");
        if (spritePathNode != null)
            item.SpritePath = spritePathNode.InnerText;

        // Process actions and effects if present
        ProcessActions(itemNode, item);
        ProcessEffects(itemNode, item);

        return item;
    }

    // Method to process ItemGear-specific nodes
    private void ProcessItemGearNode(XmlNode itemNode, ItemGear itemGear)
    {
        // Parse and assign properties specific to ItemGear
        XmlNode gearSlotNode = itemNode.SelectSingleNode("GearSlot");
        if (gearSlotNode != null && int.TryParse(gearSlotNode.InnerText, out int gearSlot))
            itemGear.GearSlot = gearSlot;
    }

    // Method to process ItemConsumable-specific nodes
    private void ProcessItemConsumableNode(XmlNode itemNode, ItemConsumable itemConsumable)
    {
        // Parse and assign properties specific to ItemConsumable
        XmlNode usesNode = itemNode.SelectSingleNode("Uses");
        if (usesNode != null && int.TryParse(usesNode.InnerText, out int uses))
        {
            itemConsumable.UsesTotal = uses;
            itemConsumable.UsesLeft = uses;
        }
            

        XmlNode typeNode = itemNode.SelectSingleNode("Type");
        if (typeNode != null)
            itemConsumable.Type = typeNode.InnerText;
    }

    // Method to process actions from an XML node
    private void ProcessActions(XmlNode itemNode, BaseItem item)
    {
        XmlNode actionsNode = itemNode.SelectSingleNode("Actions");
        if (actionsNode != null)
        {
            if (item is ItemGear gear)
            {
                XmlNodeList actionNodes = actionsNode.SelectNodes("Action");
                gear.Actions = new List<BaseAction>();
                foreach (XmlNode actionNode in actionNodes)
                {
                    BaseAction action = ProcessAction(actionNode);
                    gear.Actions.Add(action);
                }
            }
            if(item is ItemConsumable consumable)
            {
                XmlNodeList actionNodes = actionsNode.SelectNodes("Action");
                consumable.Actions = new List<BaseAction>();
                foreach (XmlNode actionNode in actionNodes)
                {
                    BaseAction action = ProcessAction(actionNode);
                    consumable.Actions.Add(action);
                }
            }
        }
    }

    // Method to process an action from an XML node
    private BaseAction ProcessAction(XmlNode actionNode)
    {
        ActionLoader loader = new ActionLoader(actionNode);
        return loader.GenerateAction();
    }

    // Method to process effects from an XML node
    private void ProcessEffects(XmlNode itemNode, BaseItem item)
    {
        XmlNode effectsNode = itemNode.SelectSingleNode("Effects");
        if (effectsNode != null)
        {
            if(item is ItemGear gear)
            {
                XmlNodeList effectNodes = effectsNode.SelectNodes("Effect");
                gear.Effects = new List<IEffect>();
                foreach (XmlNode effectNode in effectNodes)
                {
                    IEffect effect = ProcessEffect(effectNode, item.ItemName);
                    gear.Effects.Add(effect);
                }
            }           
        }
    }

    // Method to process an effect from an XML node
    private IEffect ProcessEffect(XmlNode effectNode, string source)
    {
        EffectLoader effectLoader = new EffectLoader(effectNode, source);
        return effectLoader.GenerateEffect();
    }
}
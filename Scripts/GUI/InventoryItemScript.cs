using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

public partial class InventoryItemScript : Control
{
    [Export]
    TextureRect ItemSprite;
    [Export]
    PanelContainer ItemDetails;
    [Export]
    Label ItemName;
    [Export]
    Label ItemDescription;
    [Export]
    Label ItemActions;
    [Export]
    Label ItemEffects;
    [Export]
    Label ItemUses;
    Timer timer = new Timer();
    public bool isEmpty;
    public int itemIndex;
    public BaseItem item;
    Vector2 originalPosition = Vector2.Zero;
    bool dragging = false;
    bool isMouseOver = false;
    Vector2 dragOffset = Vector2.Zero;
    public InventoryScript parent;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if(!isEmpty)
        {
            timer.Timeout += ShowDetails;
            AddChild(timer);
            ItemSprite.Texture = GD.Load<Texture2D>(item.SpritePath);
            ItemName.Text = item.ItemName;
            ItemDescription.Text = item.Description;
            if (item is ItemConsumable consumable)
            {
                ItemEffects.Text = "";
                ItemUses.Text = $"Uses:{consumable.UsesLeft}/{consumable.UsesTotal}";
                string outputActions = "Actions:";
                foreach (BaseAction action in consumable.Actions)
                {
                    outputActions += $"\n{action.ActionName}\nCost:";
                    foreach (string key in action.Cost.Keys)
                    {
                        outputActions += $"\n{key}: {action.Cost[key]}";
                    }
                    outputActions += "\nAction Effects:";
                    foreach (IEffect effect in action.Effects)
                    {
                        outputActions += $"\n{effect.EffectName}\n{effect.Description}";
                    }

                }
                ItemActions.Text = outputActions;
            }
            if (item is ItemGear gear)
            {
                ItemUses.Text = "";
                if (gear.Actions.Count > 0)
                {
                    string outputActions = "Actions:";
                    foreach (BaseAction action in gear.Actions)
                    {
                        outputActions += $"\n{action.ActionName}\nCost:";
                        foreach (string key in action.Cost.Keys)
                        {
                            outputActions += $"\n{key}: {action.Cost[key]}";
                        }
                        outputActions += "\nAction Effects:";
                        foreach (IEffect effect in action.Effects)
                        {
                            outputActions += $"\n{effect.EffectName}\n{effect.Description}";
                        }
                    }
                    ItemActions.Text = outputActions;
                }
                else
                {
                    ItemActions.Text = "";
                }
                if (gear.Effects.Count > 0)
                {
                    string outputEffects = "Item Effects:";
                    foreach (IEffect itemEffect in gear.Effects)
                    {
                        outputEffects += $"\n{itemEffect.EffectName}\n{itemEffect.Description}";
                    }
                    ItemEffects.Text = outputEffects;
                }
                else
                {
                    ItemEffects.Text = string.Empty;
                }
            }
        }
        else
        {
            ItemSprite.Texture = null;
            ItemName.Text = string.Empty;
            ItemUses.Text = string.Empty;
            ItemActions.Text = string.Empty;
            ItemEffects.Text = string.Empty;

        }

    }
    public void MouseOver()
    {
        if (!isEmpty)
        {
            isMouseOver = true;
            timer.Start(0.5);
        }
        

    }
    void ShowDetails()
    {
        if (!isEmpty)
        {
            ItemDetails.Visible = true;
            ItemDetails.Position = GlobalPosition + new Vector2(50f, 0);
        }
       
    }
    public void MouseOut()
    {
        if (!isEmpty)
        {
            isMouseOver = false;
            timer.Stop();
            ItemDetails.Visible = false;
        }

    }
    public override void _Input(InputEvent @event)
    {
        if (!isEmpty)
        {
            if (@event is InputEventMouseButton mouseButton)
            {
                if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left && isMouseOver)
                {
                    dragging = true;
                    originalPosition = Position;
                    dragOffset = mouseButton.Position - Position;


                }
                else if (!mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
                {
                    dragging = false;
                    Position = originalPosition;
                    GD.Print(GetGlobalMousePosition());
                    var itemUnderMouse = parent.GetItemAtPosition(GetGlobalMousePosition());
                    if (itemUnderMouse != null && itemUnderMouse != this)
                    {
                        var index1 = itemIndex;
                        var index2 = itemUnderMouse.itemIndex;
                        parent.SwapItems(index1, index2);
                    }
                }
            }
        }
        
    }
 
    public override void _Process(double delta)
    {
        if (dragging)
        {
            Position = GetGlobalMousePosition() - dragOffset; // Adjust for the center of the item
        }
    }
}

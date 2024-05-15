using Godot;
using System;

public partial class InventoryScript : PanelContainer
{
    [Export]
    public GridContainer InventoryGrid;
    [Export]
    public GridContainer GearGrid;

    GameManager gameManager;
    AssetManager assetManager;
    PlayerEntity player;
    PackedScene inventoryItem;
    PackedScene inventoryItemEmpty;
    bool isMouseOver = false;
    bool draggingWindow = false;
    Vector2 dragOffset = Vector2.Zero;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        assetManager = (AssetManager)GetNode("/root/AssetManager");
        player = gameManager.CurrentPlayer;
        player.Inventory.InventoryChanged += UpdateInventoryGrid;
        inventoryItem = GD.Load<PackedScene>("Templates/InventoryItem.tscn");
        inventoryItemEmpty = GD.Load<PackedScene>("Templates/InventoryItemEmpty.tscn");
        UpdateInventoryGrid();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
        {
            GD.Print("Recieved mouse left click pressed");
            // Check if mouse is over the UI element
            if (isMouseOver)
            {
                // Start dragging
                draggingWindow = true;
                dragOffset = mouseButton.Position - Position;
            }
        }
        // Check if left mouse button is released
        else if (@event is InputEventMouseButton mouseButtonRelease && !mouseButtonRelease.Pressed && mouseButtonRelease.ButtonIndex == MouseButton.Left)
        {
            // Stop dragging
            draggingWindow = false;
        }

    }
    public void OnMouseEnter()
    {
        isMouseOver = true;
    }
    public void OnMouseExit()
    {
        isMouseOver = false;
    }
    public void UpdateInventoryGrid()
    {
        ClearInventoryGrid();
        ClearGearGrid();
        for(int g = 0; g < 9; g++)
        {
            if (player.Inventory.items[g] == null)
            {
                InventoryItemScript itemInstance = (InventoryItemScript)inventoryItem.Instantiate();
                itemInstance.isEmpty = true;
                itemInstance.itemIndex = g;
                itemInstance.parent = this;
                GearGrid.GetChild(g).AddChild(itemInstance);
            }
            else
            {
                InventoryItemScript itemInstance = (InventoryItemScript)inventoryItem.Instantiate();
                itemInstance.item = player.Inventory.items[g];
                itemInstance.parent = this;
                itemInstance.itemIndex = g;
                GearGrid.GetChild(g).AddChild(itemInstance);
            }
        }
        for(int i = 9; i < player.Inventory.items.Length; i++) 
        {
            
            if (player.Inventory.items[i] == null)
            {
                InventoryItemScript itemInstance = (InventoryItemScript)inventoryItem.Instantiate();
                itemInstance.isEmpty = true;
                itemInstance.itemIndex = i;
                itemInstance.parent = this;
                InventoryGrid.GetChild(i - 9).AddChild(itemInstance);
            }
            else
            {
                InventoryItemScript itemInstance = (InventoryItemScript)inventoryItem.Instantiate();
                itemInstance.parent = this;
                itemInstance.itemIndex = i;
                itemInstance.item = player.Inventory.items[i];
                InventoryGrid.GetChild(i - 9).AddChild(itemInstance);
            }
        }
    }
    public void ClearGearGrid()
    {
        foreach (Node child in GearGrid.GetChildren())
        {
            foreach (Node childchild in child.GetChildren())
            {
                childchild.QueueFree();
            }
        }
    }
    public void ClearInventoryGrid()
    {
        foreach (Node child in InventoryGrid.GetChildren())
        {
            foreach(Node childchild in child.GetChildren())
            {
                childchild.QueueFree();
            }
        }

    }
    public InventoryItemScript GetItemAtPosition(Vector2 position)
    {
        foreach(Node child in GearGrid.GetChildren())
        {
            foreach (Node itemNode in child.GetChildren())
            {
                var item = itemNode as InventoryItemScript;
                if (item != null && item.GetGlobalRect().HasPoint(position))
                {
                    return item;
                }
            }
        }
        // Iterate through the children of the inventory grid to find the item at the specified position
        foreach (Node child in InventoryGrid.GetChildren())
        {
            foreach (Node itemNode in child.GetChildren())
            {
                var item = itemNode as InventoryItemScript;
                if (item != null && item.GetGlobalRect().HasPoint(position))
                {
                    return item;
                }
            }
        }
        return null;
    }
    public void SwapItems(int item1, int item2)
    {
        player.Inventory.MoveItem(item1, item2);
        UpdateInventoryGrid();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (draggingWindow)
        {
            Position = GetGlobalMousePosition() - dragOffset;
        }
    }
}

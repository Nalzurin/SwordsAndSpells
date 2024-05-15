using Godot;
using System;

public partial class TravelUIScript : CanvasLayer
{
	[Export]
	TextureButton InventoryButton;
	[Export]
	TextureButton CharacterButton;
	[Export]
	TextureButton JournalButton;
	[Export]
	TextureButton AbilitiesButton;
	[Export]
	PanelContainer Inventory;
	[Export]
    PanelContainer CharacterMenu;
	[Export]
	CenterContainer JournalContainer;
	[Export]
	PanelContainer AbilitiesContainer;
	PlayerEntity player;
	AssetManager assetManager;
	GameManager gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		assetManager = (AssetManager)GetNode("/root/AssetManager");
		gameManager = (GameManager)GetNode("/root/GameManager");
		player = gameManager.CurrentPlayer;
        InventoryButton.Pressed += ToggleInventory;
		CharacterButton.Pressed += ToggleCharacterMenu;
		AbilitiesButton.Pressed += ToggleAbilitiesMenu;
	}
	public void ToggleTravelUI()
	{
		this.Visible = !this.Visible;
	}
	public void ToggleAbilitiesMenu()
	{
		AbilitiesContainer.Visible = !AbilitiesContainer.Visible;
	}
	public void ToggleCharacterMenu()
	{
		CharacterMenu.Visible = !CharacterMenu.Visible;
	}
    public void ToggleInventory()
    {
		Inventory.Visible = !Inventory.Visible;
        
    }
    public override void _UnhandledInput(InputEvent @event)
    {
		if(gameManager.CurrentState == "TRAVEL")
		{
            if (@event.IsActionPressed("ui_inventory"))
            {
                ToggleInventory();
            }
            if (@event.IsActionPressed("ui_charactermenu"))
            {
                ToggleInventory();
            }
            if (@event.IsActionPressed("ui_abilitiesmenu"))
            {
                ToggleAbilitiesMenu();
            }

        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}
}

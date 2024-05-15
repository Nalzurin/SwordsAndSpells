using Godot;
using System;
using static System.Collections.Specialized.BitVector32;

public partial class CombatMenu : CanvasLayer
{
    [Export]
    PanelContainer ActionsMenuContainer;
    [Export]
    GridContainer ActionsMenuGrid;
    [Export]
    Button ActionsMenuBackButton;
    [Export]
    TextureRect EnemySprite;
    [Export]
    Button GearMenuButton;
    [Export]
    Button ItemMenuButton;
    [Export]
    Button AbilityMenuButton;
    [Export]
    Button MiscMenuButton;
    [Export]
    HSlider EnemyHealh;
    [Export]
    Label EnemyName;
    [Export]
    Label EnemyLevel;
    [Export]
    HSlider PlayerHealth;
    [Export]
    HSlider PlayerMana;
    [Export]
    HSlider PlayerStamina;
    [Export]
    Label PlayerHealthValue;
    [Export]
    Label PlayerManaValue;
    [Export]
    Label PlayerStaminaValue;

    string CurrentTurn = "Player";
    GameManager gameManager;
    AssetManager assetManager;
    public EnemyEntity Enemy;
    public PlayerEntity Player;
    // Called when the node enters the scene tree for the first time.
    PackedScene actionTemplate = GD.Load<PackedScene>("Templates/CombatActionTemplate.tscn");
    PackedScene itemTemplate = GD.Load<PackedScene>("Templates/CombatItemTemplate.tscn");
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        assetManager = (AssetManager)GetNode("/root/AssetManager");
        Player = gameManager.CurrentPlayer;
        Enemy = gameManager.CurrentEnemy.EnemyData;
        Enemy.Characteristics.HealthChanged += UpdateEnemyHealth;
        Player.Characteristics.HealthChanged += UpdatePlayerStatus;
        Player.Characteristics.ManaChanged += UpdatePlayerStatus;
        Player.Characteristics.StaminaChanged += UpdatePlayerStatus;
        EnemySprite.Texture = Enemy.GetSprite();
        EnemyName.Text = Enemy.EntityName;
        EnemyLevel.Text = "Level: " + Enemy.Characteristics.CharacterLevel;
        ActionsMenuBackButton.Pressed += HideActionsMenu;
        GearMenuButton.Pressed += GearMenuButtonPress;
        AbilityMenuButton.Pressed += AbilityMenuButtonPress;
        ItemMenuButton.Pressed += ItemMenuButtonPress;
        UpdatePlayerStatus();
        UpdateEnemyHealth();
    }
    public void HideActionsMenu()
    {
        ActionsMenuContainer.Visible = false;
    }
    public void UpdateEnemyHealth()
    {
        EnemyHealh.MaxValue = Enemy.Characteristics.HealthMax;
        EnemyHealh.Value = Enemy.Characteristics.HealthCurrent;
    }
    public void UpdatePlayerStatus()
    {
        PlayerHealth.MaxValue = Player.Characteristics.HealthMax;
        PlayerHealth.Value = Player.Characteristics.HealthCurrent;
        PlayerMana.MaxValue = Player.Characteristics.ManaMax;
        PlayerMana.Value = Player.Characteristics.ManaCurrent;
        PlayerStamina.MaxValue = Player.Characteristics.StaminaMax;
        PlayerStamina.Value = Player.Characteristics.StaminaCurrent;
        PlayerHealthValue.Text = $"{Player.Characteristics.HealthCurrent}/{Player.Characteristics.HealthMax}";
        PlayerStaminaValue.Text = $"{Player.Characteristics.StaminaCurrent}/{Player.Characteristics.StaminaMax}";
        PlayerManaValue.Text = $"{Player.Characteristics.ManaCurrent}/{Player.Characteristics.ManaMax}";

    }
    public void UseAction(BaseEntity caller, string target, BaseAction action)
    {
        GD.Print("Doing action");
        if (target == "Player")
        {
            action.Do_Action(caller, Player);
        }
        if (target == "Enemy")
        {
            action.Do_Action(caller, Enemy);
        }
        EndTurn();
    }
    public void OpenActionMenu(string tab)
    {
        ClearActionsMenu();
        ActionsMenuContainer.Visible = true;
        switch (tab)
        {
            case "Ability":
                PopulateActionMenuAbility();
                break;
            case "Gear":
                PopulateActionMenuGear();
                break;
            case "Item":
                PopulateActionMenuItem();
                break;
            case "Misc":
                break;
            default:
                break;
        }
    }
    public void PopulateActionMenuGear()
    {
        for (int i = 0; i < 9; i++)
        {
            ItemGear gear = (ItemGear)Player.Inventory.items[i];
            if (gear != null && gear.Actions.Count > 0)
            {
                foreach (BaseAction action in gear.Actions)
                {
                    CombatActionTemplate instance = (CombatActionTemplate)actionTemplate.Instantiate();
                    instance.action = action;
                    instance.combatMenu = this;
                    ActionsMenuGrid.AddChild(instance);
                }
            }
        }
    }
    public void PopulateActionMenuItem()
    {
        for (int i = 9; i < 39; i++)
        {
            ItemConsumable consumable = (ItemConsumable)Player.Inventory.items[i];
            if (consumable != null && consumable.Actions.Count > 0)
            {
                CombatItemTemplate instance = (CombatItemTemplate)itemTemplate.Instantiate();
                instance.item = consumable;
                instance.combatMenu = this;
                ActionsMenuGrid.AddChild(instance);
            }
        }
    }
    public void PopulateActionMenuAbility()
    {
        foreach (BaseAbility ability in Player.Abilities.abilities)
        {
            if (ability.IsActive)
            {
                foreach (BaseAction action in ability.Actions)
                {
                    CombatActionTemplate instance = (CombatActionTemplate)actionTemplate.Instantiate();
                    instance.action = action;
                    instance.combatMenu = this;
                    ActionsMenuGrid.AddChild(instance);
                }
            }
        }
    }
    public void GearMenuButtonPress()
    {
        OpenActionMenu("Gear");
    }
    public void AbilityMenuButtonPress()
    {
        OpenActionMenu("Ability");

    }
    public void ItemMenuButtonPress()
    {
        OpenActionMenu("Item");
    }
    public void ClearActionsMenu()
    {
        foreach (Node child in ActionsMenuGrid.GetChildren())
        {
            child.QueueFree();
        }
    }
    public void EndTurn()
    {
        ActionsMenuContainer.Visible = false;
        if (CurrentTurn == "Player")
        {
            if (Enemy.Characteristics.HealthCurrent <= 0)
            {
                EndCombat("Victory");
                return;
            }
            CurrentTurn = "Enemy";
            gameManager.CurrentEnemy.EnemyData.Effects.UpdateEffects();
        }
        else
        {
            if (Player.Characteristics.HealthCurrent <= 0)
            {
                EndCombat("Defeat");
                return;
            }
            CurrentTurn = "Player";
            gameManager.CurrentPlayer.Effects.UpdateEffects();
        }
    }
    public void EndCombat(string status)
    {
        if (status == "Victory")
        {
            GD.Print("Victory!");
            gameManager.CurrentEnemy.DeleteSelf();
            gameManager.CurrentEnemy = null;
            gameManager.EndCombat();
            this.QueueFree();
        }
        else
        {
            GD.Print("Defeat!");
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

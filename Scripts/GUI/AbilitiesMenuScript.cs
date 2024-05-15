using Godot;
using System;

public partial class AbilitiesMenuScript : PanelContainer
{
    bool isMouseOver = false;
    bool draggingWindow = false;
    Vector2 dragOffset = Vector2.Zero;
    GameManager gameManager;
    AssetManager assetManager;
    PlayerEntity player;
    [Export]
    Button StrengthButton;
    [Export]
    Button DexterityButton;
    [Export]
    Button VitalityButton;
    [Export]
    Button IntelligenceButton;
    [Export]
    Button LuckButton;
    [Export]
    VBoxContainer AbilityList;
    [Export]
    TextureRect AbilityDetailsIcon;
    [Export]
    Label AbilityDetailsName;
    [Export]
    Label AbilityDetailsDescription;
    [Export]
    Label AbilityDetailsStatus;
    [Export]
    Label AbilityDetailsActionEffect;
    [Export]
    Label AbilityDetailsActionsEffectsDetails;
    [Export]
    Label AbilityPrerequisites;
    // Called when the node enters the scene tree for the first time.
    PackedScene abilityTemplate = GD.Load<PackedScene>("Templates/GUIAbilityTemplate.tscn");
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        assetManager = (AssetManager)GetNode("/root/AssetManager");
        player = gameManager.CurrentPlayer;
        SetAbilityList("Dexterity");
        StrengthButton.Pressed += SetStrengthList;
        DexterityButton.Pressed += SetDexterityList;
        VitalityButton.Pressed += SetVitalityList;
        IntelligenceButton.Pressed += SetIntelligenceList;
        LuckButton.Pressed += SetLuckList;

    }

    void SetStrengthList()
    {
        SetAbilityList("Strength");
    }
    void SetDexterityList()
    {
        SetAbilityList("Dexterity");
    }
    void SetVitalityList()
    {
        SetAbilityList("Vitality");
    }
    void SetIntelligenceList()
    {
        SetAbilityList("Intelligence");
    }
    void SetLuckList()
    {
        SetAbilityList("Luck");
    }

    public void SetAbilityDetails(BaseAbility ability)
    {
        if (ability != null)
        {
            AbilityDetailsIcon.Texture = GD.Load<Texture2D>(ability.SpritePath);
            AbilityDetailsName.Text = ability.AbilityName;
            AbilityDetailsDescription.Text = ability.Description;
            foreach(string prereq in ability.Prerequisites.Keys)
            {
                AbilityPrerequisites.Text = $"\n{prereq}: {ability.Prerequisites[prereq]}";
            }
            
            if (ability.IsActive)
            {
                AbilityDetailsStatus.Text = "Active";
                AbilityDetailsActionEffect.Text = "Actions";
                string output = "";
                foreach (BaseAction action in ability.Actions)
                {
                    output += $"{action.ActionName}:\n{action.Description}\nTarget: {action.TargetType}\nCost:";
                    foreach (string cost in action.Cost.Keys)
                    {
                        output += $"\n{cost}: {action.Cost[cost]}";
                    }
                    output += "\nEffects:";
                    foreach (IEffect effect in action.Effects)
                    {
                        output += $"\n{effect.EffectName}\n{effect.Description}\nDuration: {effect.Duration} rounds\nIs harmful: {effect.IsHarmful}";
                    }
                }
                AbilityDetailsActionsEffectsDetails.Text = output;
            }
            else
            {
                AbilityDetailsStatus.Text = "Passive";
                AbilityDetailsActionEffect.Text = "Effects";
            }


        }
        else
        {
            GD.Print("ability is null");
        }
    }

    public void SetAbilityList(string attribute)
    {
        ClearAbilityList();
        foreach (BaseAbility ability in assetManager.abilities.Values)
        {
            if (ability.Attribute == attribute)
            {
                GD.Print(ability.ID);
                GUIAbilityTemplate abilityContainer = (GUIAbilityTemplate)abilityTemplate.Instantiate();
                abilityContainer.ability = ability.ID;
                abilityContainer.abilitiesMenuScript = this;
                AbilityList.AddChild(abilityContainer);
            }
        }
    }
    public void ClearAbilityList()
    {
        foreach (Node child in AbilityList.GetChildren())
        {
            child.QueueFree();
        }
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
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (draggingWindow)
        {
            Position = GetGlobalMousePosition() - dragOffset;
        }
    }
}

using Godot;
using System;

public partial class CharacterMenuScript : PanelContainer
{

    bool isMouseOver = false;
    bool draggingWindow = false;
    Vector2 dragOffset = Vector2.Zero;
    GameManager gameManager;
    AssetManager assetManager;
    PlayerEntity player;
    [Export]
    Label PlayerNameLabel;
    [Export]
    TextureRect PlayerSprite;
    [Export]
    Label HealthLabel;
    [Export]
    HSlider HealthSlider;
    [Export]
    Label StaminaLabel;
    [Export]
    HSlider StaminaSlider;
    [Export]
    Label ManaLabel;
    [Export]
    HSlider ManaSlider;
    [Export]
    Label CharacterLevelLabel;
    [Export]
    Label CharacterExpLabel;
    [Export]
    HSlider CharacterSlider;
    [Export]
    Label StrengthLabel;
    [Export]
    Label StrengthExpLabel;
    [Export]
    HSlider StrengthSlider;
    [Export]
    Label DexterityLabel;
    [Export]
    Label DexterityExpLabel;
    [Export]
    HSlider DexteritySlider;
    [Export]
    Label VitalityLabel;
    [Export]
    Label VitalityExpLabel;
    [Export]
    HSlider VitalitySlider;
    [Export]
    Label IntelligenceLabel;
    [Export]
    Label IntelligenceExpLabel;
    [Export]
    HSlider IntelligenceSlider;
    [Export]
    Label LuckLabel;
    [Export]
    Label LuckExpLabel;
    [Export]
    HSlider LuckSlider;
    [Export]
    Label MeleeDamageMultLabel;
    [Export]
    Label RangedDamageMultLabel;
    [Export]
    Label MagicDamageMultLabel;
    [Export]
    Label LuckMultLabel;
    [Export]
    Label DodgeChanceLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        assetManager = (AssetManager)GetNode("/root/AssetManager");
        player = gameManager.CurrentPlayer;
        player.Characteristics.HealthChanged += UpdateHealth;
        player.Characteristics.StaminaChanged += UpdateStamina;
        player.Characteristics.ManaChanged += UpdateMana;
        player.Characteristics.AttributesChanged += UpdateAttributes;
        PlayerNameLabel.Text = player.EntityName;
        PlayerSprite.Texture = player.GetSprite();
        UpdateHealth();
        UpdateStamina();
        UpdateMana();
        UpdateAttributes();
    }
    public void UpdateHealth()
    {
        HealthLabel.Text = player.Characteristics.HealthCurrent + "/" + player.Characteristics.HealthMax;
        HealthSlider.MaxValue = player.Characteristics.HealthMax;
        HealthSlider.Value = player.Characteristics.HealthCurrent;
    }
    public void UpdateStamina()
    {
        StaminaLabel.Text = player.Characteristics.StaminaCurrent + "/" + player.Characteristics.StaminaMax;
        StaminaSlider.MaxValue = player.Characteristics.StaminaMax;
        StaminaSlider.Value = player.Characteristics.StaminaCurrent;
    }
    public void UpdateMana()
    {
        ManaLabel.Text = player.Characteristics.ManaCurrent + "/" + player.Characteristics.ManaMax;
        ManaSlider.MaxValue = player.Characteristics.ManaMax;
        ManaSlider.Value = player.Characteristics.ManaCurrent;
    }
    public void UpdateAttributes()
    {
        CharacterLevelLabel.Text = player.Characteristics.CharacterLevel.ToString();
        CharacterExpLabel.Text = player.Experience.CharacterExp + "/" + player.Experience.Formula(player.Characteristics.CharacterLevel + 1);
        CharacterSlider.MaxValue = player.Experience.Formula(player.Characteristics.CharacterLevel + 1);
        CharacterSlider.Value = player.Experience.CharacterExp;

        StrengthLabel.Text = player.Characteristics.StrengthCurrent.ToString();
        StrengthExpLabel.Text = player.Experience.StrengthExp + "/" + player.Experience.Formula(player.Characteristics.StrengthBase + 1);
        StrengthSlider.MaxValue = player.Experience.Formula(player.Characteristics.StrengthBase + 1);
        StrengthSlider.Value = player.Experience.StrengthExp;

        DexterityLabel.Text = player.Characteristics.DexterityCurrent.ToString();
        DexterityExpLabel.Text = player.Experience.DexterityExp + "/" + player.Experience.Formula(player.Characteristics.DexterityBase + 1);
        DexteritySlider.MaxValue = player.Experience.Formula(player.Characteristics.DexterityBase + 1);
        DexteritySlider.Value = player.Experience.DexterityExp;

        VitalityLabel.Text = player.Characteristics.VitalityCurrent.ToString();
        VitalityExpLabel.Text = player.Experience.VitalityExp + "/" + player.Experience.Formula(player.Characteristics.VitalityBase + 1);
        VitalitySlider.MaxValue = player.Experience.Formula(player.Characteristics.VitalityBase + 1);
        VitalitySlider.Value = player.Experience.VitalityExp;

        IntelligenceLabel.Text = player.Characteristics.IntelligenceCurrent.ToString();
        IntelligenceExpLabel.Text = player.Experience.IntelligenceExp + "/" + player.Experience.Formula(player.Characteristics.IntelligenceBase + 1);
        IntelligenceSlider.MaxValue = player.Experience.Formula(player.Characteristics.IntelligenceBase + 1);
        IntelligenceSlider.Value = player.Experience.IntelligenceExp;

        LuckLabel.Text = player.Characteristics.LuckCurrent.ToString();
        LuckExpLabel.Text = player.Experience.LuckExp + "/" + player.Experience.Formula(player.Characteristics.LuckBase + 1);
        LuckSlider.MaxValue = player.Experience.Formula(player.Characteristics.LuckBase + 1);
        LuckSlider.Value = player.Experience.LuckExp;

        MeleeDamageMultLabel.Text = player.Characteristics.MeleeDamageMult.ToString();
        RangedDamageMultLabel.Text = player.Characteristics.RangedDamageMult.ToString();
        MagicDamageMultLabel.Text = player.Characteristics.MagicDamageMult.ToString();
        LuckMultLabel.Text = player.Characteristics.LuckMult.ToString();
        DodgeChanceLabel.Text = player.Characteristics.DodgeChance.ToString();
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

using Godot;
using System;

public partial class CombatActionTemplate : MarginContainer
{
	[Export]
	TextureRect ActionIcon;
	[Export]
	Label ActionName;
	[Export]
	Label ActionCost;
	[Export]
	Button ActionButton;
	public BaseAction action;
    GameManager gameManager;
    AssetManager assetManager;
	public CombatMenu combatMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(action.ActionName);
		gameManager = (GameManager)GetNode("/root/GameManager");
		assetManager = (AssetManager)GetNode("/root/AssetManager");
		ActionIcon.Texture = GD.Load<Texture2D>(action.SpritePath);
		ActionName.Text = action.ActionName;
		ActionCost.Text = "";
		if (action.Cost.ContainsKey("Stamina")) ActionCost.Text += $"SP:{action.Cost["Stamina"]}";
		if (action.Cost.ContainsKey("Mana")) ActionCost.Text += $"MP:{action.Cost["Mana"]}";
		if (action.Cost.ContainsKey("Health")) ActionCost.Text += $"HP:{action.Cost["Health"]}";
		if (action.Cost.ContainsKey("Stamina") && gameManager.CurrentPlayer.Characteristics.StaminaCurrent < action.Cost["Stamina"]) ActionButton.Disabled = true;
        if (action.Cost.ContainsKey("Mana") && gameManager.CurrentPlayer.Characteristics.ManaCurrent < action.Cost["Mana"]) ActionButton.Disabled = true;
        if (action.Cost.ContainsKey("Health") && gameManager.CurrentPlayer.Characteristics.HealthCurrent < action.Cost["Health"]) ActionButton.Disabled = true;
		ActionButton.ButtonUp += OnPress;
    }
	public void OnPress()
	{
		if(action.TargetType == "Self")
		{
            combatMenu.UseAction(combatMenu.Player, "Player", action);
            foreach (IEffect effect in action.Effects)
            {
                if (effect.Duration == 0)
                {
                    combatMenu.Player.Statistics.UpdateHealthHealed(effect.Value);

                }
                else
                {
                    combatMenu.Player.Statistics.UpdateHealthHealed(effect.Value * effect.Duration);
                }


            }
        }
		else
		{
            combatMenu.UseAction(combatMenu.Player, "Enemy", action);
            foreach (IEffect effect in action.Effects)
            {
                if (effect.Duration == 0)
                {
                    combatMenu.Player.Statistics.UpdateDamageDealt(-effect.Value);

                }
                else
                {
                    combatMenu.Player.Statistics.UpdateDamageDealt(-effect.Value * effect.Duration);
                }


            }
        }
		
	}

}

using Godot;
using System;
using static System.Collections.Specialized.BitVector32;

public partial class CombatItemTemplate : MarginContainer
{
    [Export]
    TextureRect ItemIcon;
    [Export]
    Label ItemName;
    [Export]
    Label ItemUses;
    [Export]
    Button ItemButton;
    public ItemConsumable item;
    GameManager gameManager;
    AssetManager assetManager;
    public CombatMenu combatMenu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print(item.ItemName);
        gameManager = (GameManager)GetNode("/root/GameManager");
        assetManager = (AssetManager)GetNode("/root/AssetManager");
        ItemIcon.Texture = GD.Load<Texture2D>(item.SpritePath);
        ItemName.Text = item.ItemName;
        ItemUses.Text = $"{item.UsesLeft / item.UsesTotal}";
        ItemButton.Pressed += OnPress;
    }
    public void OnPress()
    {
        if (item.Type == "Food" || item.Type == "Potion")
        {
            combatMenu.UseAction(combatMenu.Player, "Player", item.Actions[0]);
            foreach (IEffect effect in item.Actions[0].Effects)
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
            combatMenu.UseAction(combatMenu.Player, "Enemy", item.Actions[0]);
            foreach (IEffect effect in item.Actions[0].Effects)
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

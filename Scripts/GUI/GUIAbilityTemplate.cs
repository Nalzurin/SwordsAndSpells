using Godot;
using System;

public partial class GUIAbilityTemplate : MarginContainer
{
	[Export]
	TextureRect AbilityIcon;
	[Export]
	Label AbilityName;
	[Export]
	Label AbilityStatus;
	public string ability;
    GameManager gameManager;
    AssetManager assetManager;
	public AbilitiesMenuScript abilitiesMenuScript;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GD.Print(ability);
        gameManager = (GameManager)GetNode("/root/GameManager");
        assetManager = (AssetManager)GetNode("/root/AssetManager");
		AbilityIcon.Texture = GD.Load<Texture2D>(assetManager.abilities[ability].SpritePath);
		AbilityName.Text = assetManager.abilities[ability].AbilityName;
		if (assetManager.abilities[ability].IsActive)
		{
			AbilityStatus.Text = "Active";

        }
		else
		{
            AbilityStatus.Text = "Passive";
        }
		
    }
	public void OnPress()
	{
		abilitiesMenuScript.SetAbilityDetails(assetManager.abilities[ability]);
	}

}

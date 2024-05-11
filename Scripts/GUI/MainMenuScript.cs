using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;




public partial class MainMenuScript : CanvasLayer
{
	[Export]
	CenterContainer CharacterMenu;
	[Export]
	CenterContainer MainMenu;
	[Export]
	CenterContainer WorldMenu;
	[Export]
	CenterContainer SettingsMenu;
	[Export]
	Button GameExitButton;
	[Export]
	Button CharacterMenuButton;
	[Export]
	Button CharacterMenuBackButton;
	[Export]
	Button CharacterMenuNewButton;
	[Export]
	Button CharacterMenuDeleteButton;
	[Export]
	Button CharacterMenuSelectButton;
	[Export]
	Label SelectedCharacterLabel;
	[Export]
	TextureRect SelectedCharacterIcon;
	[Export]
	Label SelectedWorldLabel;
	[Export]
	ItemList characterList;
	[Export]
	CenterContainer CharacterCreator;
	[Export]
	VBoxContainer CharacterCreatorFirst;
    [Export]
    VBoxContainer CharacterCreatorSecond;
    [Export]
    VBoxContainer CharacterCreatorThird;
	[Export]
	Button NewCharacterFirstBackButton;
    [Export]
    Button NewCharacterFirstNextButton;
    [Export]
    Button NewCharacterSecondBackButton;
    [Export]
    Button NewCharacterSecondNextButton;
    [Export]
    Button NewCharacterThirdBackButton;
    [Export]
    Button NewCharacterThirdCreateButton;
	[Export]
	TextEdit NewCharacterName;
	[Export]
	Button SelectCharacterImage;
	[Export]
	TextureRect NewCharacterSprite;
	[Export]
	ItemList AbilityList;
	[Export]
	Label AvailableAbilitiesCountLabel;
	[Export]
	ItemList WeaponList;
    [Export]
    Button WorldMenuButton;
    [Export]
    Button WorldMenuBackBackButton;
	[Export]
	Button WorldMenuSelectButton;
	[Export]
	Button WorldMenuDeleteButton;
	[Export]
	Button WorldMenuNewButton;
	[Export]
	ItemList WorldList;
	[Export]
	CenterContainer WorldCreator;
	[Export]
	TextEdit NewWorldName;
	[Export]
	TextEdit NewWorldSeed;
	[Export]
	TextEdit NewWorldSize;
	[Export]
	TextEdit NewWorldMinBiomeSize;
	[Export]
	TextEdit NewWorldLacunarity;
	[Export]
	TextEdit NewWorldOctaves;
	[Export]
	TextEdit NewWorldFrequency;
	[Export]
	Button NewWorldBackButton;
	[Export]
	Button NewWorldCreateButton;

    PlayerEntity selectedPlayer;
	AssetManager assetManager;
	SaveSystem saveSystem;
	// Called when the node enters the scene tree for the first time.
	[Export]
	FileDialog FileDialog;
	[Export]
	Button PlayButton;
	int availableAbilitiesCount = 5;
	private GameManager gameManager;
	public override void _Ready()
	{
		saveSystem = (SaveSystem)GetNode("/root/SaveSystem");
		assetManager = (AssetManager)GetNode("/root/AssetManager");
		gameManager = (GameManager)GetNode("/root/GameManager");
		GameExitButton.Pressed += ExitGame;
		CharacterMenuButton.Pressed += ToggleVisiblityCharacters;
		CharacterMenuBackButton.Pressed += ToggleVisiblityCharacters;
		CharacterMenuDeleteButton.Pressed += DeleteCharacter;
		CharacterMenuSelectButton.Pressed += SelectCharacter;
		CharacterMenuNewButton.Pressed += NewCharacterFirst;
		NewCharacterFirstNextButton.Pressed += NewCharacterSecond;
		NewCharacterSecondNextButton.Pressed += NewCharacterThird;
		NewCharacterThirdCreateButton.Pressed += NewCharacterCreateCharacter;
		NewCharacterFirstBackButton.Pressed += NewCharacterFirstBack;
		NewCharacterSecondBackButton.Pressed += NewCharacterSecondBack;
		NewCharacterThirdBackButton.Pressed += NewCharacterThirdBack;
		SelectCharacterImage.Pressed += SelectCharacterImageFunc;

        FileDialog.FileSelected += FileDialog_FileSelected;
		AbilityList.MultiSelected += AbilitySelected;
		WeaponList.ItemSelected += WeaponSelected;
		AvailableAbilitiesCountLabel.Text = availableAbilitiesCount.ToString();
		WorldMenuButton.Pressed += ToggleVisibilityWorlds;
		WorldMenuDeleteButton.Pressed += DeleteWorld;
		WorldMenuSelectButton.Pressed += SelectWorld;
		WorldMenuNewButton.Pressed += NewWorld;
		WorldMenuBackBackButton.Pressed += ToggleVisibilityWorlds;
		NewWorldBackButton.Pressed += NewWorldBack;
		NewWorldCreateButton.Pressed += CreateWorld;
        PlayButton.Pressed += Play;
    }

    private void PlayButton_Pressed()
    {
        throw new NotImplementedException();
    }

    private string newCharName = "";
	private string characterSpritePath = "";
	private Texture2D characterSprite;
	private void NewCharacterFirst()
	{
		CharacterCreator.Visible = true;
		CharacterMenu.Visible = false;
		CharacterCreatorFirst.Visible = true;

	}
	private void SelectCharacterImageFunc()
	{ 
		FileDialog.AddFilter("*.png, *.jpg", "Images");
		FileDialog.UseNativeDialog = true;
		FileDialog.FileMode = (FileDialog.FileModeEnum)FileMode.Open;
		FileDialog.Access = FileDialog.AccessEnum.Filesystem;
		FileDialog.Visible = true;
	}
    private void FileDialog_FileSelected(string path)
    {
        string destinationFolder = "User/Sprites/Characters/";
        string fileName = System.IO.Path.GetFileName(path);
        string destinationPath = System.IO.Path.Combine(destinationFolder, fileName);
        try
        {
            System.IO.File.Copy(path, destinationPath, overwrite: true);

            Image image = Image.LoadFromFile(destinationPath);
            ImageTexture imageTexture = ImageTexture.CreateFromImage(image);

            characterSpritePath = destinationPath;
            characterSprite = imageTexture;
            NewCharacterSprite.Texture = imageTexture;


        }
        catch (Exception e)
        {
            GD.Print("Error copying file: " + e.Message);
        }

    }
    private void NewCharacterFirstBack()
	{
        CharacterMenu.Visible = true;
		CharacterCreator.Visible = false;
        CharacterCreatorFirst.Visible = false;
		NewCharacterName.Text = string.Empty;
		NewCharacterSprite.Texture = null;
		characterSpritePath = string.Empty;
		newCharName = string.Empty;
    }
    private Dictionary<int, string> abilitiesId = new Dictionary<int, string>();
    private List<string> selectedAbilities = new List<string>();
    private void NewCharacterSecond()
	{
        AbilityList.Clear();
        abilitiesId.Clear();
		selectedAbilities.Clear();
		availableAbilitiesCount = 5;
		AvailableAbilitiesCountLabel.Text = availableAbilitiesCount.ToString();
        foreach (BaseAbility ability in assetManager.abilities.Values)
        {
			int index = AbilityList.AddItem(ability.AbilityName, GD.Load<Texture2D>(ability.SpritePath));
            abilitiesId.Add(index, ability.ID);
			AbilityList.SetItemTooltip(index, ability.Description);
        }

        newCharName = NewCharacterName.Text;
        CharacterCreatorFirst.Visible = false;
		CharacterCreatorSecond.Visible = true;
    }
	
    private void AbilitySelected(long index, bool selected)
    {
		if (selected)
		{
			if (!selectedAbilities.Contains(abilitiesId[(int)index]))
			{
                selectedAbilities.Add(abilitiesId[(int)index]);
                availableAbilitiesCount--;
            }

		}
		else
		{
            availableAbilitiesCount++;
            selectedAbilities.Remove(abilitiesId[(int)index]);

		}
        
        AvailableAbilitiesCountLabel.Text = availableAbilitiesCount.ToString();

    }

    private void NewCharacterSecondBack()
    {
		
        CharacterCreatorFirst.Visible = true;
        CharacterCreatorSecond.Visible = false;
    }
    private Dictionary<int, string> weaponsId = new Dictionary<int, string>();
	private string selectedWeaponId = "";

    private void NewCharacterThird()
	{
		weaponsId.Clear();
		selectedWeaponId = "";
		WeaponList.Clear();
		foreach(BaseItem item in assetManager.items.Values)
		{
			if(item is ItemGear weapon && weapon.GearSlot == 0)
			{
                int index = WeaponList.AddItem(weapon.ItemName, GD.Load<Texture2D>(weapon.SpritePath));
                weaponsId.Add(index, weapon.ID);
                WeaponList.SetItemTooltip(index, weapon.Description);
            }
		}
		CharacterCreatorSecond.Visible = false;
		CharacterCreatorThird.Visible = true;
	}
    private void WeaponSelected(long index)
    {
        selectedWeaponId = weaponsId[(int)index];
    }
    private void NewCharacterThirdBack()
    {
        CharacterCreatorSecond.Visible = true;
        CharacterCreatorThird.Visible = false;
    }
    private void NewCharacterCreateCharacter()
	{
		PlayerEntity newPlayer = new PlayerEntity("Player_"+newCharName, newCharName,"Player",characterSpritePath,"Player",new Effects(),new Abilities(), new Actions(), new Characteristics(), new Experience(), new Inventory());
		foreach(string ability in selectedAbilities)
		{
			newPlayer.Abilities.AddAbility(assetManager.GetAbility(ability));

        }
		newPlayer.Inventory.AddItem(assetManager.GetItem(selectedWeaponId));
		newPlayer.Inventory.SetParent(newPlayer);
		newPlayer.Effects.SetParent(newPlayer);
		newPlayer.Abilities.SetParent(newPlayer);
		newPlayer.Actions.SetParent(newPlayer);
		newPlayer.Experience.SetParent(newPlayer);
		newPlayer.Characteristics.SetParent(newPlayer);
		saveSystem.SaveCharacter(newPlayer);
		newCharName = "";
		NewCharacterName.Text = "";
		NewCharacterSprite.Texture = null;
		CharacterCreator.Visible = false;
		CharacterCreatorThird.Visible = false;
		ListCharacters();
		CharacterMenu.Visible = true;
	}
	Dictionary<int, string> characterIds = new Dictionary<int, string>();
	private void SelectCharacter()
	{
		selectedPlayer = assetManager.GetCharacter(characterIds[characterList.GetSelectedItems()[0]]);
		ToggleVisiblityCharacters();
		SelectedCharacterLabel.Text = selectedPlayer.EntityName;
		SelectedCharacterIcon.Texture = selectedPlayer.GetSprite();

    }
	private void DeleteCharacter()
	{
		if(SelectedCharacterLabel.Text == characterList.GetItemText(characterList.GetSelectedItems()[0]))
		{
			selectedPlayer = null;
			SelectedCharacterLabel.Text = "";
			SelectedCharacterIcon.Texture = null;
		}
		saveSystem.DeleteCharacter(assetManager.GetCharacter(characterIds[characterList.GetSelectedItems()[0]]));
		ToggleVisiblityCharacters();

    }
	private void ToggleVisiblityCharacters()
	{
		MainMenu.Visible = !MainMenu.Visible;
		CharacterMenu.Visible = !CharacterMenu.Visible;
		ListCharacters();
	}
	private void ListCharacters()
	{
		characterList.Clear();
		characterIds.Clear();
		foreach(PlayerEntity entity in assetManager.playerCharacters.Values)
		{
			characterIds.Add(characterList.AddItem(entity.EntityName, entity.GetSprite(), true),entity.ID);
		}
    }
	private void ExitGame()
	{
		GetTree().Quit();
	}
	
	Dictionary<int, string> worldIds = new Dictionary<int, string>();

	WorldFile selectedWorld;
    private void SelectWorld()
    {
        selectedWorld = assetManager.GetWorld(worldIds[WorldList.GetSelectedItems()[0]]);
        SelectedWorldLabel.Text = selectedWorld.Name;
        ToggleVisibilityWorlds();
        
    }
    private void DeleteWorld()
    {
        if (SelectedWorldLabel.Text == worldIds[WorldList.GetSelectedItems()[0]])
        {
            selectedWorld = null;
            SelectedWorldLabel.Text = "";
        }
        saveSystem.DeleteWorld(assetManager.GetWorld(worldIds[WorldList.GetSelectedItems()[0]]));
        ToggleVisibilityWorlds();

    }
	private void NewWorld()
	{
		WorldMenu.Visible = false;
		WorldCreator.Visible = true;
	}
	private void CreateWorld()
	{

		int seed = 0, size = 1024, octaves = 4, minbiomesize = 20;
		float frequency = 0.001f, lacunarity = 1f;
		if(NewWorldSeed.Text != "") int.TryParse(NewWorldSeed.Text, out seed);
        if (NewWorldSize.Text != "") int.TryParse(NewWorldSize.Text, out size);
        if (NewWorldOctaves.Text != "") int.TryParse(NewWorldOctaves.Text, out octaves);
        if (NewWorldMinBiomeSize.Text != "") int.TryParse(NewWorldMinBiomeSize.Text, out minbiomesize);
        if (NewWorldFrequency.Text != "") float.TryParse(NewWorldFrequency.Text, out frequency);
        if (NewWorldLacunarity.Text != "") float.TryParse(NewWorldLacunarity.Text, out lacunarity);
		saveSystem.SaveWorld(new WorldFile("World"+NewWorldName.Text, NewWorldName.Text, Vector2I.Zero, new WorldGenSettings(size, seed, frequency,octaves,lacunarity,minbiomesize), new Dictionary<Vector2I, BaseEntity>()));
		NewWorldSeed.Text = "";
		NewWorldSize.Text = "";
		NewWorldOctaves.Text = "";
		NewWorldMinBiomeSize.Text = "";
		NewWorldFrequency.Text = "";
		NewWorldLacunarity.Text = "";
		NewWorldName.Text = "";
		NewWorldBack();

    }
	private void NewWorldBack()
	{
		WorldCreator.Visible = false;
		WorldMenu.Visible = true;
		ListWorlds();
	}
    private void ToggleVisibilityWorlds()
    {
        MainMenu.Visible = !MainMenu.Visible;
        WorldMenu.Visible = !WorldMenu.Visible;
        ListWorlds();
    }
    private void ListWorlds()
    {
        worldIds.Clear();
        WorldList.Clear();
        foreach (WorldFile world in assetManager.worlds.Values)
        {
            worldIds.Add(WorldList.AddItem(world.Name, null, true), world.ID);
        }
    }

	private void Play()
	{
		gameManager.CurrentPlayer = selectedPlayer;
		gameManager.CurrentWorld = selectedWorld;
		gameManager.SwitchState("TRAVEL");
        SwitchScene("res://Scenes/World.tscn");
    }
    private void SwitchScene(string scenePath)
    {
        SceneTree sceneTree = GetTree();
        PackedScene scene = (PackedScene)ResourceLoader.Load(scenePath);
        sceneTree.CallDeferred("change_scene_to_packed", scene);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (!string.IsNullOrEmpty(NewWorldName.Text))
		{
			NewWorldCreateButton.Disabled = false;

		}
		else
		{
            NewWorldCreateButton.Disabled = true;

        }
        if (characterList.IsAnythingSelected())
		{
			CharacterMenuDeleteButton.Disabled = false;
			CharacterMenuSelectButton.Disabled = false;
		}
		else
		{
			CharacterMenuDeleteButton.Disabled = true;
			CharacterMenuSelectButton.Disabled = true;
        }
		if(!string.IsNullOrEmpty(characterSpritePath) && !string.IsNullOrEmpty(NewCharacterName.Text))
		{
			NewCharacterFirstNextButton.Disabled = false;
		}
		else
		{
            NewCharacterFirstNextButton.Disabled = true;
		}
		if(AbilityList.GetSelectedItems().Length <= 5 && AbilityList.GetSelectedItems().Length > 0)
		{
			NewCharacterSecondNextButton.Disabled = false;
		}
		else
		{
			NewCharacterSecondNextButton.Disabled = true;
		}
		if(selectedWeaponId != null)
		{
			NewCharacterThirdCreateButton.Disabled = false;
		}
		else
		{
			NewCharacterThirdCreateButton.Disabled = true;
		}
		if(selectedWorld != null && selectedPlayer != null)
		{
			PlayButton.Disabled = false;
		}
		else
		{
			PlayButton.Disabled = true;
		}
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public partial class EnemyEntityLoader : Node
{
    private const string EnemiesFolder = "Assets/Enemies/";
    private AssetManager _assets;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _assets = (AssetManager)GetNode("/root/AssetManager");
        LoadEnemies();
        _assets.DebugListEnemies(); // Assuming you have a method like this to debug list of enemies
    }

    // Method to load enemies from XML files in the specified folder
    private void LoadEnemies()
    {
        // Get all XML files from the specified folder
        string[] files = Directory.GetFiles(EnemiesFolder, "*.xml");
        foreach (string file in files)
        {
            LoadEnemy(file);
        }
    }

    // Method to load an enemy from a specific XML file
    private void LoadEnemy(string filePath)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNodeList enemyNodes = xmlDoc.GetElementsByTagName("EntityEnemy");
            foreach (XmlNode node in enemyNodes)
            {
                EnemyEntity enemy = ProcessEnemy(node);
                _assets.AddEnemy(enemy); // Assuming AssetManager has an AddEnemy method
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading enemy from file: {filePath}");
            GD.PrintErr(e.Message);
        }
    }

    // Method to process an enemy from an XML node
    private EnemyEntity ProcessEnemy(XmlNode enemyNode)
    {
        // Create a new EnemyEntity instance
        string id = enemyNode.SelectSingleNode("ID")?.InnerText ?? string.Empty;
        string name = enemyNode.SelectSingleNode("Name")?.InnerText ?? string.Empty;
        string description = enemyNode.SelectSingleNode("Description")?.InnerText ?? string.Empty;
        string spritePath = enemyNode.SelectSingleNode("SpritePath")?.InnerText ?? string.Empty;
        string rarity = enemyNode.SelectSingleNode("Rarity")?.InnerText ?? string.Empty;
        string targetType = "Enemy"; // Always "Enemy" according to the schema
        List<string> Biomes = new List<string>();
        try
        {
            Biomes = ProcessBiomeSpawns(enemyNode.SelectSingleNode("Biomes"));
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading biomes");
            GD.PrintErr(e.Message);
        }
        Characteristics characteristics = new Characteristics();
        // Process characteristics
        try
        {
            characteristics = ProcessCharacteristics(enemyNode.SelectSingleNode("Characteristics"));
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading characteristics");
            GD.PrintErr(e.Message);
        }

        Effects effects = new Effects();
        // Process effects
        try
        {
            effects = ProcessEffects(enemyNode.SelectSingleNode("Effects"), name);
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading effects");
            GD.PrintErr(e.Message);
        }
        Actions actions = new Actions();
        try
        {
            actions = ProcessActions(enemyNode.SelectSingleNode("Actions"));
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading actions");
            GD.PrintErr(e.Message);
        }
        Abilities abilities = new Abilities();
        try
        {
            abilities = ProcessAbilities(enemyNode.SelectSingleNode("Abilities"));
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading abilities");
            GD.PrintErr(e.Message);
        }
        EnemyEntity entity = new EnemyEntity(id, name, description, "Assets/Sprites/" + spritePath, targetType, rarity, Biomes, effects, abilities, actions, characteristics);
        entity.Characteristics.SetParent(entity);
        entity.Effects.SetParent(entity);
        entity.Actions.SetParent(entity);
        entity.Abilities.SetParent(entity);
        // Create and return the EnemyEntity instance
        return entity;
    }
    public List<string> ProcessBiomeSpawns(XmlNode Biomes)
    {
        List<string> list = new List<string>();
        if (Biomes != null)
        {
            foreach (XmlNode biomeNode in Biomes.SelectNodes("Biome"))
            {
                string biomeID = biomeNode?.InnerText ?? string.Empty;
                if (_assets.GetBiome(biomeID) != null)
                {
                    list.Add(biomeID);
                }

            }
        }

        return list;
    }
    // Method to process characteristics from an XML node
    private Characteristics ProcessCharacteristics(XmlNode characteristicsNode)
    {
        if (characteristicsNode == null)
            return new Characteristics();

        int health = 0, stamina = 0, mana = 0, level = 0, strength = 0, dexterity = 0, vitality = 0, intelligence = 0, luck = 0;
        int.TryParse(characteristicsNode.SelectSingleNode("Health").InnerText, out health);
        int.TryParse(characteristicsNode.SelectSingleNode("Stamina").InnerText, out stamina);
        int.TryParse(characteristicsNode.SelectSingleNode("Mana").InnerText, out mana);
        int.TryParse(characteristicsNode.SelectSingleNode("Level").InnerText, out level);
        int.TryParse(characteristicsNode.SelectSingleNode("Strength").InnerText, out strength);
        int.TryParse(characteristicsNode.SelectSingleNode("Dexterity").InnerText, out dexterity);
        int.TryParse(characteristicsNode.SelectSingleNode("Vitality").InnerText, out vitality);
        int.TryParse(characteristicsNode.SelectSingleNode("Intelligence").InnerText, out intelligence);
        int.TryParse(characteristicsNode.SelectSingleNode("Luck").InnerText, out luck);

        return new Characteristics(level, health, health, stamina, stamina, mana, mana, strength, strength, dexterity, dexterity, vitality, vitality, intelligence, intelligence, luck, luck, null);
    }

    // Method to process effects from an XML node
    private Effects ProcessEffects(XmlNode effectsNode, string entityName)
    {
        List<IEffect> effects = new List<IEffect>();
        if (effectsNode != null)
        {
            foreach (XmlNode effectNode in effectsNode.SelectNodes("Effect"))
            {
                EffectLoader loader = new EffectLoader(effectNode, entityName);
                IEffect effect = loader.GenerateEffect();
                effects.Add(effect);

            }
        }
        return new Effects(effects, null);
    }

    // Method to process an action from an XML node
    private Actions ProcessActions(XmlNode actionsNode)
    {
        Actions actions = new Actions();
        if (actionsNode == null)
            return actions;

        foreach (XmlNode actionNode in actionsNode.SelectNodes("Action"))
        {
            ActionLoader loader = new ActionLoader(actionNode);

            BaseAction action = loader.GenerateAction();
            actions.AddAction(action);
        }

        return actions;
    }

    // Method to process abilities from an XML node
    private Abilities ProcessAbilities(XmlNode abilitiesNode)
    {
        List<BaseAbility> abilities = new List<BaseAbility>();
        if (abilitiesNode != null)
        {
            foreach (XmlNode abilityNode in abilitiesNode.SelectNodes("Ability"))
            {
                BaseAbility ability = _assets.GetAbility(abilityNode.InnerText);
                if (ability != null)
                {
                    abilities.Add(ability);
                }
            }
        }
        return new Abilities(abilities, null);
    }

}

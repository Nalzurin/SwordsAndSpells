using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public partial class AbilityLoader : Node
{
    const bool Export = true;
    private const string AbilitiesFolder = "res://Assets/Abilities/";
    private const string UserAbilitiesFolder = "user://Assets/Abilities/";
    private AssetManager _assets;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _assets = (AssetManager)GetNode("/root/AssetManager");
        LoadAbilities();
        _assets.DebugListAbilities();
    }

    // Method to load abilities from XML files in the specified folder
    private void LoadAbilities()
    {
        string[] files;
        if (Export)
        {
            files = DirAccess.GetFilesAt(UserAbilitiesFolder);
        }
        else
        {
            files = DirAccess.GetFilesAt(AbilitiesFolder);
        }
        foreach (string file in files)
        {
            if (file.EndsWith(".xml"))
            {
                LoadAbility(file);
            }
           
        }
    }

    // Method to load an ability from a specific XML file
    private void LoadAbility(string filePath)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (Export)
            {
                xmlDoc.Load(ProjectSettings.GlobalizePath(UserAbilitiesFolder + filePath));
            }
            else
            {
                xmlDoc.Load(ProjectSettings.GlobalizePath(AbilitiesFolder + filePath));
            }
           

            XmlNodeList abilityNodes = xmlDoc.GetElementsByTagName("Ability");
            foreach (XmlNode node in abilityNodes)
            {
                BaseAbility ability = ProcessAbility(node);
                _assets.AddAbility(ability);
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error loading ability from file: {filePath}");
            GD.PrintErr(e.Message);
        }
    }

    // Method to process an ability from an XML node
    private BaseAbility ProcessAbility(XmlNode abilityNode)
    {
        // Create an ability object and assign common properties
        BaseAbility ability = new BaseAbility();

        XmlNode idNode = abilityNode.SelectSingleNode("ID");
        if (idNode != null)
            ability.ID = idNode.InnerText;

        XmlNode nameNode = abilityNode.SelectSingleNode("Name");
        if (nameNode != null)
            ability.AbilityName = nameNode.InnerText;

        XmlNode isActiveNode = abilityNode.SelectSingleNode("IsActive");
        if (isActiveNode != null && bool.TryParse(isActiveNode.InnerText, out bool isActive))
            ability.IsActive = isActive;

        XmlNode descNode = abilityNode.SelectSingleNode("Description");
        if (descNode != null)
            ability.Description = descNode.InnerText;

        XmlNode spritePathNode = abilityNode.SelectSingleNode("SpritePath");
        if (spritePathNode != null)
            ability.SpritePath = spritePathNode.InnerText;

        XmlNode attributeNode = abilityNode.SelectSingleNode("Attribute");
        if (attributeNode != null)
            ability.Attribute = attributeNode.InnerText;
        // Process prerequisites
        XmlNode prerequisitesNode = abilityNode.SelectSingleNode("Prerequisites");
        if (prerequisitesNode != null)
            ability.Prerequisites = ProcessPrerequisites(prerequisitesNode);

        // Process either actions (for active abilities) or effects (for passive abilities)
        ProcessActions(abilityNode, ability);
        ProcessEffects(abilityNode, ability);

        return ability;
    }

    // Process prerequisites from an XML node
    private Dictionary<string, int> ProcessPrerequisites(XmlNode prerequisitesNode)
    {
        Dictionary<string, int> prerequisites = new Dictionary<string, int>();

        foreach (XmlNode node in prerequisitesNode.ChildNodes)
        {
            switch (node.Name)
            {
                case "CharacterLevel":
                    if (int.TryParse(node.InnerText, out int level))
                        prerequisites.Add("CharacterLevel", level);
                    break;
                case "Strength":
                    if (int.TryParse(node.InnerText, out int strength))
                        prerequisites.Add("Strength", strength);
                    break;
                case "Dexterity":
                    if (int.TryParse(node.InnerText, out int dexterity))
                        prerequisites.Add("Dexterity", dexterity);
                    break;
                case "Vitality":
                    if (int.TryParse(node.InnerText, out int vitality))
                        prerequisites.Add("Vitality", vitality);
                    break;
                case "Intelligence":
                    if (int.TryParse(node.InnerText, out int intelligence))
                        prerequisites.Add("Intelligence", intelligence);
                    break;
                case "Luck":
                    if (int.TryParse(node.InnerText, out int luck))
                        prerequisites.Add("Luck", luck);
                    break;
                default:
                    GD.PrintErr($"Unknown prerequisite: {node.Name}");
                    break;
            }
        }

        return prerequisites;
    }

    // Method to process actions from an XML node
    private void ProcessActions(XmlNode abilityNode, BaseAbility ability)
    {
        XmlNode actionsNode = abilityNode.SelectSingleNode("Actions");
        ability.Actions = new List<BaseAction>();
        if (actionsNode != null)
        {
            XmlNodeList actionNodes = actionsNode.SelectNodes("Action");
            foreach (XmlNode actionNode in actionNodes)
            {
                BaseAction action = ProcessAction(actionNode);
                ability.Actions.Add(action);
            }

        }
    }

    // Method to process an action from an XML node
    private BaseAction ProcessAction(XmlNode actionNode)
    {
        ActionLoader loader = new ActionLoader(actionNode);
        return loader.GenerateAction();
    }

    // Method to process effects from an XML node
    private void ProcessEffects(XmlNode abilityNode, BaseAbility ability)
    {
        XmlNode effectsNode = abilityNode.SelectSingleNode("Effects");
        ability.Effects = new List<IEffect>();
        if (effectsNode != null)
        {

            XmlNodeList effectNodes = effectsNode.SelectNodes("Effect");
           
            foreach (XmlNode effectNode in effectNodes)
            {
                IEffect effect = ProcessEffect(effectNode, ability.AbilityName);
                ability.Effects.Add(effect);
            }
        }
    }

    // Method to process an effect from an XML node
    private IEffect ProcessEffect(XmlNode effectNode, string source)
    {
        EffectLoader effectLoader = new EffectLoader(effectNode, source);
        return effectLoader.GenerateEffect();
    }
}

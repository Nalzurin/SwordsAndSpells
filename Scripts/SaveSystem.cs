using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public partial class SaveSystem : Node
{

    readonly string charactersFilePath = "Saved/Characters/";
    readonly string worldsFilePath = "Saved/Worlds/";
    AssetManager assetManager;
    public override void _Ready()
    {
        assetManager = (AssetManager)GetNode("/root/AssetManager");
    }
    public void LoadCharacters()
    {
        foreach (string filePath in Directory.GetFiles(charactersFilePath, "*.xml"))
        {
            // Load the XML file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Parse the XML file and create a PlayerEntity
            PlayerEntity playerEntity = ParsePlayerEntity(xmlDoc);

            // Add the PlayerEntity to the list
            if (playerEntity != null)
            {
                if(assetManager.GetCharacter(playerEntity.ID) != null)
                {
                    assetManager.UpdateCharacter(playerEntity.ID, playerEntity);
                }
                else
                {
                    assetManager.AddCharacter(playerEntity);
                }
               
            }
        }
        assetManager.DebugListPlayerCharacters();
    }
    // Parse an XML document and return a PlayerEntity instance
    private PlayerEntity ParsePlayerEntity(XmlDocument xmlDoc)
    {
        // Get the root element
        XmlElement root = xmlDoc.DocumentElement;

        if (root == null || root.Name != "EntityPlayer")
        {
            GD.PrintErr("Invalid XML file format: missing root element 'EntityPlayer'.");
            return null;
        }

        // Extract data from the XML document
        string id = root.SelectSingleNode("ID")?.InnerText ?? "";
        string name = root.SelectSingleNode("Name")?.InnerText ?? "";
        string description = root.SelectSingleNode("Description")?.InnerText ?? "";
        string spritePath = root.SelectSingleNode("SpritePath")?.InnerText ?? "";
        string targetType = root.SelectSingleNode("TargetType")?.InnerText ?? "Player";

        // Load Effects and Actions
        Effects effects = LoadEffects(root.SelectSingleNode("Effects"));
        Actions actions = LoadActions(root.SelectSingleNode("Actions"));

        // Load Abilities
        Abilities abilities = LoadAbilities(root.SelectSingleNode("Abilities"));

        // Load Characteristics
        Characteristics characteristics = LoadCharacteristics(root.SelectSingleNode("Characteristics"));

        // Load Experience
        Experience experience = LoadExperience(root.SelectSingleNode("Experience"));
        Inventory inventory = LoadInventory(root.SelectSingleNode("Inventory"));
        // Create and return a PlayerEntity instance
        PlayerEntity playerEntity = new PlayerEntity(id, name, description, spritePath, targetType, effects, abilities, actions, characteristics, experience, inventory);
        playerEntity.Effects.SetParent(playerEntity);
        playerEntity.Actions.SetParent(playerEntity);
        playerEntity.Abilities.SetParent(playerEntity);
        playerEntity.Experience.SetParent(playerEntity);
        playerEntity.Inventory.SetParent(playerEntity);
        playerEntity.Characteristics.SetParent(playerEntity);
        playerEntity.Inventory.ApplyActions();
        playerEntity.Inventory.ApplyEffects();
        playerEntity.Experience.LevelUpAll();
        playerEntity.Abilities.ApplyAbilities();
        return playerEntity;
    }
    private Inventory LoadInventory(XmlNode inventoryNode)
    {
        BaseItem[] inventory = new BaseItem[39];
        if(inventoryNode != null)
        {
            foreach(XmlNode node in inventoryNode.ChildNodes)
            {
                BaseItem item = assetManager.GetItem(node.SelectSingleNode("ID").InnerText);
                if(item != null)
                {
                    inventory[int.Parse(node.SelectSingleNode("Index").InnerText)] = item;
                }
            }
        }
        return new Inventory(inventory, null);
    }
    // Helper method to load Effects
    private Effects LoadEffects(XmlNode effectsNode)
    {
        List<IEffect> effectsList = new List<IEffect>();
        if (effectsNode != null)
        {
            foreach (XmlNode effectNode in effectsNode.ChildNodes)
            {
                // Use the custom class EffectLoader to load each effect
                IEffect effect = new EffectLoader(effectNode, "Player").GenerateEffect();
                if (effect != null)
                {
                    effectsList.Add(effect);
                }
            }
        }

        // Return an Effects instance
        return new Effects(effectsList, null); // Set parent as needed
    }

    // Helper method to load Actions
    private Actions LoadActions(XmlNode actionsNode)
    {
        List<BaseAction> actionsList = new List<BaseAction>();
        if (actionsNode != null)
        {
            foreach (XmlNode actionNode in actionsNode.ChildNodes)
            {
                // Use the custom class ActionLoader to load each action
                BaseAction action = new ActionLoader(actionNode).GenerateAction();
                if (action != null)
                {
                    actionsList.Add(action);
                }
            }
        }

        // Return an Actions instance
        return new Actions(actionsList, null); // Set parent as needed
    }

    // Helper method to load Abilities
    private Abilities LoadAbilities(XmlNode abilitiesNode)
    {
        List<BaseAbility> abilitiesList = new List<BaseAbility>();
        if (abilitiesNode != null)
        {
            foreach (XmlNode abilityNode in abilitiesNode.ChildNodes)
            {
                // Load each ability (you can define how to load abilities)
                BaseAbility ability = LoadAbility(abilityNode);
                if (ability != null)
                {
                    abilitiesList.Add(ability);
                }
            }
        }

        // Return an Abilities instance
        return new Abilities(abilitiesList, null); // Set parent as needed
    }

    // Helper method to load a single ability
    private BaseAbility LoadAbility(XmlNode abilityNode)
    {
        BaseAbility baseAbility = assetManager.GetAbility(abilityNode.InnerText);

        return baseAbility;
    }

    // Helper method to load Characteristics
    private Characteristics LoadCharacteristics(XmlNode characteristicsNode)
    {
        // Parse characteristics from the XML node
        int health = int.Parse(characteristicsNode.SelectSingleNode("HealthCurrent")?.InnerText ?? "0");
        int stamina = int.Parse(characteristicsNode.SelectSingleNode("StaminaCurrent")?.InnerText ?? "0");
        int mana = int.Parse(characteristicsNode.SelectSingleNode("ManaCurrent")?.InnerText ?? "0");

        // Return a Characteristics instance
        return new Characteristics(1, 50, health, 50, stamina, 50, mana, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, null);
    }

    // Helper method to load Experience
    private Experience LoadExperience(XmlNode experienceNode)
    {
        int characterExp = int.Parse(experienceNode.SelectSingleNode("CharacterExp")?.InnerText ?? "0");
        int strengthExp = int.Parse(experienceNode.SelectSingleNode("StrengthExp")?.InnerText ?? "0");
        int dexterityExp = int.Parse(experienceNode.SelectSingleNode("DexterityExp")?.InnerText ?? "0");
        int vitalityExp = int.Parse(experienceNode.SelectSingleNode("VitalityExp")?.InnerText ?? "0");
        int intelligenceExp = int.Parse(experienceNode.SelectSingleNode("IntelligenceExp")?.InnerText ?? "0");
        int luckExp = int.Parse(experienceNode.SelectSingleNode("LuckExp")?.InnerText ?? "0");

        // Return an Experience instance
        return new Experience(null, characterExp, strengthExp, dexterityExp, vitalityExp, intelligenceExp, luckExp);
    }
    public void SaveCharacter(PlayerEntity player)
    {
        // Create an XmlWriterSettings object to configure the XML writing process
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true, // To format the XML file nicely
            IndentChars = "    ",
            NewLineOnAttributes = false
        };

        // Define the file path where the character will be saved
        string filePath = Path.Combine(charactersFilePath, $"{player.EntityName}.xml");

        // Create an XmlWriter instance and start writing to the specified file path
        using (XmlWriter writer = XmlWriter.Create(filePath, settings))
        {
            // Start the XML document
            writer.WriteStartDocument();

            // Start the root element "EntityPlayer"
            writer.WriteStartElement("EntityPlayer");

            // Write the ID, Name, Description, SpritePath, and TargetType
            WriteElement(writer, "ID", player.ID);
            WriteElement(writer, "Name", player.EntityName);
            WriteElement(writer, "Description", player.Description);
            WriteElement(writer, "SpritePath", player.SpritePath);
            WriteElement(writer, "TargetType", player.TargetType);

            // Save Effects
            WriteEffects(writer, player.Effects);

            // Save Actions
            WriteActions(writer, player.Actions);

            // Save Abilities
            WriteAbilities(writer, player.Abilities);

            // Save Characteristics
            WriteCharacteristics(writer, player.Characteristics);

            // Save Experience
            WriteExperience(writer, player.Experience);

            // Save Inventory
            WriteInventory(writer, player.Inventory);

            // End the root element "EntityPlayer"
            writer.WriteEndElement();

            // End the XML document
            writer.WriteEndDocument();
        }
        LoadCharacters();
    }

    // Helper method to write an XML element
    private void WriteElement(XmlWriter writer, string elementName, string value)
    {
        writer.WriteStartElement(elementName);
        writer.WriteString(value);
        writer.WriteEndElement();
    }

    // Helper method to write Effects
    private void WriteEffects(XmlWriter writer, Effects effects)
    {
        effects.WriteEffects(writer, effects);
    }

    // Helper method to write Actions
    private void WriteActions(XmlWriter writer, Actions actions)
    {
        actions.WriteActions(writer, actions);

    }

    // Helper method to write Abilities
    private void WriteAbilities(XmlWriter writer, Abilities abilities)
    {
        writer.WriteStartElement("Abilities");
        // Iterate through the abilities and write each one
        foreach (var ability in abilities.abilities)
        {
            writer.WriteStartElement("Ability");
            // Serialize the ability ID or properties as required
            writer.WriteString(ability.ID);
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }

    // Helper method to write Characteristics
    private void WriteCharacteristics(XmlWriter writer, Characteristics characteristics)
    {
        writer.WriteStartElement("Characteristics");
        // Write the properties as XML elements
        WriteElement(writer, "Level", characteristics.CharacterLevel.ToString());
        WriteElement(writer, "HealthCurrent", characteristics.HealthCurrent.ToString());
        WriteElement(writer, "StaminaCurrent", characteristics.StaminaCurrent.ToString());
        WriteElement(writer, "ManaCurrent", characteristics.ManaCurrent.ToString());
        WriteElement(writer, "StrengthCurrent", characteristics.StrengthCurrent.ToString());
        WriteElement(writer, "DexterityCurrent", characteristics.DexterityCurrent.ToString());
        WriteElement(writer, "VitalityCurrent", characteristics.VitalityCurrent.ToString());
        WriteElement(writer, "IntelligenceCurrent", characteristics.IntelligenceCurrent.ToString());
        WriteElement(writer, "LuckCurrent", characteristics.LuckCurrent.ToString());
        writer.WriteEndElement();
    }

    // Helper method to write Experience
    private void WriteExperience(XmlWriter writer, Experience experience)
    {
        writer.WriteStartElement("Experience");
        WriteElement(writer, "CharacterExp", experience.CharacterExp.ToString());
        WriteElement(writer, "StrengthExp", experience.StrengthExp.ToString());
        WriteElement(writer, "DexterityExp", experience.DexterityExp.ToString());
        WriteElement(writer, "VitalityExp", experience.VitalityExp.ToString());
        WriteElement(writer, "IntelligenceExp", experience.IntelligenceExp.ToString());
        WriteElement(writer, "LuckExp", experience.LuckExp.ToString());
        writer.WriteEndElement();
    }

    // Helper method to write Inventory
    private void WriteInventory(XmlWriter writer, Inventory inventory)
    {
        writer.WriteStartElement("Inventory");
        // Iterate through the inventory items and write each one
        for (int i = 0; i < inventory.items.Length; i++)
        {
            var item = inventory.items[i];
            if (item != null)
            {
                writer.WriteStartElement("Item");
                WriteElement(writer, "Index", i.ToString());
                WriteElement(writer, "ID", item.ID);
                // You can also add serialization logic for other properties of BaseItem as required
                writer.WriteEndElement();
            }
        }
        writer.WriteEndElement();
    }

    public void DeleteCharacter(PlayerEntity player)
    {
        // Define the file path for the character files
        string characterFilePath = "Saved/Characters/";

        // Check if the directory exists
        if (!Directory.Exists(characterFilePath))
        {
            GD.PrintErr("The characters directory does not exist.");
            return;
        }

        // Find the corresponding XML file
        string xmlFileToDelete = null;
        foreach (string filePath in Directory.GetFiles(characterFilePath, "*.xml"))
        {
            // Load the XML file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Get the root element
            XmlElement root = xmlDoc.DocumentElement;

            if (root != null && root.Name == "EntityPlayer")
            {
                // Check the ID and Name elements
                string id = root.SelectSingleNode("ID")?.InnerText;
                string name = root.SelectSingleNode("Name")?.InnerText;

                // If either the ID or Name matches the characterIdOrName, mark this file for deletion
                if (id == player.ID || name == player.EntityName)
                {
                    xmlFileToDelete = filePath;
                    break; // We found the matching file; break the loop
                }
            }
        }

        // If an XML file was found
        if (xmlFileToDelete != null)
        {
            // Delete the XML file
            File.Delete(xmlFileToDelete);
            GD.Print($"Deleted character file: {xmlFileToDelete}");

            // Remove the PlayerEntity instance from the AssetManager
            assetManager.DeleteCharacter(player.ID);
        }
        else
        {
            GD.PrintErr("Character not found.");
        }
    }
    public void LoadWorlds()
    {
        string directoryPath = "User/Worlds/";
        string[] filePaths = Directory.GetFiles(directoryPath, "*.xml");

        foreach (string filePath in filePaths)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNode worldNode = xmlDoc.SelectSingleNode("World");
            if (worldNode != null)
            {
                WorldFile worldData = new WorldFile();
                XmlNode idNode = worldNode.SelectSingleNode("ID");
                if (idNode != null)
                {
                    worldData.ID = idNode.InnerText;
                }
                XmlNode nameNode = worldNode.SelectSingleNode("Name");
                if (nameNode != null)
                {
                    worldData.Name = nameNode.InnerText;
                }

                XmlNode worldGenSettingsNode = worldNode.SelectSingleNode("WorldGenSettings");
                if (worldGenSettingsNode != null)
                {
                    worldData.Settings = new WorldGenSettings();
                    worldData.Settings.Size = int.Parse(worldGenSettingsNode.SelectSingleNode("Size").InnerText);
                    worldData.Settings.Seed = int.Parse(worldGenSettingsNode.SelectSingleNode("Seed").InnerText);
                    worldData.Settings.Lacunarity = float.Parse(worldGenSettingsNode.SelectSingleNode("Lacunarity").InnerText);
                    worldData.Settings.MinBiomeSize = int.Parse(worldGenSettingsNode.SelectSingleNode("MinimumBiomeSize").InnerText);
                    worldData.Settings.Octaves = int.Parse(worldGenSettingsNode.SelectSingleNode("Octaves").InnerText);
                    worldData.Settings.Frequency = float.Parse(worldGenSettingsNode.SelectSingleNode("Frequency").InnerText);
                }
                Dictionary<Vector2I, BaseEntity> savedEntities = new Dictionary<Vector2I, BaseEntity>();
                XmlNodeList enemyPositionNodes = worldNode.SelectNodes("EnemiesPositions/EnemyPosition");
                if (enemyPositionNodes != null)
                {
                    foreach (XmlNode enemyPositionNode in enemyPositionNodes)
                    {

                        int X = int.Parse(enemyPositionNode.SelectSingleNode("X").InnerText);
                        int Y = int.Parse(enemyPositionNode.SelectSingleNode("Y").InnerText);
                        string Id = enemyPositionNode.SelectSingleNode("EnemyId").InnerText;
                        savedEntities.Add(new Vector2I(X, Y), assetManager.GetEnemy(Id));
                    }
                }
                worldData.SavedEntities = savedEntities;

                XmlNode spawnNode = worldNode.SelectSingleNode("Spawn");
                if (spawnNode != null)
                {
                    worldData.WorldSpawn = new Vector2I(int.Parse(spawnNode.SelectSingleNode("X").InnerText), int.Parse(spawnNode.SelectSingleNode("Y").InnerText));
                }
                if(assetManager.worlds.ContainsKey(worldData.ID))
                {
                    assetManager.UpdateWorld(worldData.ID, worldData);
                }
                else
                {
                    assetManager.AddWorld(worldData);
                }
                
                assetManager.DebugListWorlds();
            }
        }
    }
    public void SaveWorld(WorldFile world)
    {
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true, // To format the XML file nicely
            IndentChars = "    ",
            NewLineOnAttributes = false
        };
        string filePath = Path.Combine("User/Worlds/", $"{world.Name}.xml");
        using (XmlWriter writer = XmlWriter.Create(filePath, settings))
        {
            writer.WriteStartDocument();           
            writer.WriteStartElement("World");
            WriteElement(writer, "ID", world.ID);
            WriteElement(writer, "Name", world.Name);
            writer.WriteStartElement("WorldGenSettings");
            WriteElement(writer, "Size", world.Settings.Size.ToString());
            WriteElement(writer, "Seed", world.Settings.Seed.ToString());
            WriteElement(writer, "Lacunarity", world.Settings.Lacunarity.ToString());
            WriteElement(writer, "MinimumBiomeSize", world.Settings.MinBiomeSize.ToString());
            WriteElement(writer, "Octaves", world.Settings.Octaves.ToString());
            WriteElement(writer, "Frequency", world.Settings.Frequency.ToString());
            writer.WriteEndElement(); 
            writer.WriteStartElement("EnemiesPositions");
            foreach (var position in world.SavedEntities)
            {
                writer.WriteStartElement("EnemyPosition");
                WriteElement(writer, "X", position.Key.X.ToString());
                WriteElement(writer, "Y", position.Key.Y.ToString());
                WriteElement(writer, "EnemyId", position.Value.ID.ToString()); 
                writer.WriteEndElement(); 
            }
            writer.WriteEndElement(); 
            writer.WriteStartElement("Spawn");
            WriteElement(writer, "X", world.WorldSpawn.X.ToString());
            WriteElement(writer, "Y", world.WorldSpawn.Y.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
        LoadWorlds();
    }
    public void DeleteWorld(WorldFile world)
    {
        // Define the file path for the world files
        string worldFilePath = "User/Worlds/";

        // Check if the directory exists
        if (!Directory.Exists(worldFilePath))
        {
            GD.PrintErr("The characters directory does not exist.");
            return;
        }

        // Find the corresponding XML file
        string xmlFileToDelete = null;
        foreach (string filePath in Directory.GetFiles(worldFilePath, "*.xml"))
        {
            // Load the XML file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Get the root element
            XmlElement root = xmlDoc.DocumentElement;

            if (root != null && root.Name == "World")
            {
                // Check the ID and Name elements
                string id = root.SelectSingleNode("ID")?.InnerText;
                string name = root.SelectSingleNode("Name")?.InnerText;

                // If either the ID or Name matches the worldIdOrName, mark this file for deletion
                if (id == world.ID || name == world.Name)
                {
                    xmlFileToDelete = filePath;
                    break; // We found the matching file; break the loop
                }
            }
        }

        // If an XML file was found
        if (xmlFileToDelete != null)
        {
            // Delete the XML file
            File.Delete(xmlFileToDelete);
            GD.Print($"Deleted world file: {xmlFileToDelete}");

            // Remove the WorldFile instance from the AssetManager
            assetManager.DeleteWorld(world.ID);
        }
        else
        {
            GD.PrintErr("World not found.");
        }
    }
}

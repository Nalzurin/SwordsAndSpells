using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public partial class AssetManager : Node
{
    public Dictionary<string, Biome> biomes = new Dictionary<string, Biome>();
    public Dictionary<string, BaseItem> items = new Dictionary<string, BaseItem>();
    public Dictionary<string, EnemyEntity> enemies = new Dictionary<string, EnemyEntity>();
    public Dictionary<string, PlayerEntity> playerCharacters = new Dictionary<string, PlayerEntity>();
    public Dictionary<string, BaseAbility> abilities = new Dictionary<string, BaseAbility>();
    public Dictionary<string, WorldFile> worlds = new Dictionary<string, WorldFile>();
    TileSet WorldTileSet = new TileSet();

    public void SetWorldTileSet(TileSet worldTileSet)
    {
        WorldTileSet = worldTileSet;
        ResourceSaver.Save(WorldTileSet, "Assets/Sprites/Tilesets/WorldTileSet.tres");
    }
    public TileSet GetWorldTileSet()
    {
        return WorldTileSet;
    }
    public void AddWorld(WorldFile world)
    {
        worlds.Add(world.ID, world);
    }
    public void AddBiome(Biome biome)
    {
        biomes.Add(biome.ID, biome);
    }
    public void AddItem(BaseItem item)
    {
        items.Add(item.ID, item);
    }
    public void AddCharacter(PlayerEntity character)
    {
        playerCharacters.Add(character.ID, character);
    }
    public void AddAbility(BaseAbility ability)
    {
        abilities.Add(ability.ID, ability);
    }
    public void AddEnemy(EnemyEntity entity)
    {
        enemies.Add(entity.ID, entity);
    }
    public void SetBiomeTileSetSourceId(string BiomeID, int SourceId)
    {
        biomes[BiomeID].TilesetSourceId = SourceId;
    }
    public WorldFile GetWorld(string Id)
    {
        if(worlds.ContainsKey(Id)) return worlds[Id];
        return null;
    }
    public Dictionary<string, Biome> GetBiomes()
    {
        return biomes;
    }
    public Dictionary<string, BaseItem> GetItems()
    {
        return items;
    }
    public Biome GetBiome(string id)
    {
        if (biomes.ContainsKey(id)) return biomes[id];
        return null;
    }
    public PlayerEntity GetCharacter(string id)
    {
        if(playerCharacters.ContainsKey(id)) return playerCharacters[id];
        return null;
    }
    public void UpdateCharacter(string id, PlayerEntity character)
    {
        if (playerCharacters.ContainsKey(id))
        {
            playerCharacters[id] = character;
        }
    }
    public void UpdateWorld(string id, WorldFile world)
    {
        if (worlds.ContainsKey(id))
        {
            worlds[id] = world;
        }
    }
    public BaseItem GetItem(string id)
    {
        if (items.ContainsKey(id)) return items[id];
        return null;
    }
    public EnemyEntity GetEnemy(string id)
    {
        if (enemies.ContainsKey(id)) return enemies[id];
        return null;
    }
    public BaseAbility GetAbility(string id)
    {
        if (abilities.ContainsKey(id)) return abilities[id];
        return null;
    }
    public Dictionary<string, PlayerEntity> DeleteCharacter(string id)
    {
        playerCharacters.Remove(id);
        return playerCharacters;
    }
    public void DeleteWorld(string id)
    {
        if(worlds.ContainsKey(id)) worlds.Remove(id);
    }
    public void DebugListBiomes()
    {
        foreach (Biome biome in biomes.Values)
        {
            GD.Print($"{biome.ID}\nName: {biome.Name}\nDescription: {biome.Description}\nMoisture Range:[{biome.MoistureRange[0]};{biome.MoistureRange[1]}]\nTemperature Range:[{biome.TemperatureRange[0]};{biome.TemperatureRange[1]}]\nAltitude Range:[{biome.AltitudeRange[0]};{biome.AltitudeRange[1]}]\nIs Walkable: {biome.IsWalkable}\nPriority: {biome.Priority}\nTilesetSourceID: {biome.TilesetSourceId}");
        }
    }
    public void DebugListItems()
    {
        foreach (BaseItem item in items.Values)
        {
            if (item is ItemConsumable itemConsumable)
            {
                string output = $"{itemConsumable.ID}\n Name: {itemConsumable.Name}\nDescription: {itemConsumable.Description}\nRarity: {itemConsumable.Rarity}\nItem level: {itemConsumable.ItemLevel}\nUses: {itemConsumable.UsesTotal}\nType:{itemConsumable.Type}\nActions:";
                foreach (BaseAction action in itemConsumable.Actions)
                {
                    output += $"\n{action.ActionName}";
                }
                GD.Print(output);
            }
            if (item is ItemGear itemGear)
            {
                string output = $"{itemGear.ID}\n Name: {itemGear.Name}\nDescription: {itemGear.Description}\nRarity: {itemGear.Rarity}\nItem level: {itemGear.ItemLevel}\nSlot: {itemGear.GearSlot}\nEffects:";
                foreach (IEffect effect in itemGear.Effects)
                {
                    output += $"\n{effect.EffectName}";
                }
                output += "\nActions";
                foreach (BaseAction action in itemGear.Actions)
                {
                    output += $"\n{action.ActionName}";
                }
                GD.Print(output);
            }
        }
    }
    public void DebugListAbilities()
    {
        foreach (BaseAbility ability in abilities.Values)
        {
            string output = $"{ability.ID}\nName: {ability.AbilityName}\nDescription: {ability.Description}]\nIsActive: {ability.IsActive}\n";
            if (ability.Actions.Count != 0)
            {
                output += "Actions:";
                foreach (BaseAction action in ability.Actions)
                {
                    output += $"{action.ActionName}\n";
                }
            }
            if (ability.Effects.Count != 0)
            {
                output += "Effects:";
                foreach (IEffect effect in ability.Effects)
                {
                    output += $"{effect.EffectName}\n";
                }
            }

            GD.Print(output);

        }
    }
    public void DebugListEnemies()
    {
        foreach (EnemyEntity entity in enemies.Values)
        {
            string output = $"{entity.ID}\nName: {entity.EntityName}\nDescription: {entity.Description}\nRarity: {entity.Rarity}\nBiomes:";
            foreach (string biome in entity.BiomeSpawns)
            {
                output += $"\n{biome}";
            }
            List<IEffect> effects = entity.Effects.GetActiveEffects();
            output += "\nEffects: ";
            if (effects.Count != 0)
            {
                foreach (IEffect effect in effects)
                {
                    output += $"\n{effect.EffectName}";
                }
            }
            output += "\nAbilities:";
            if (entity.Abilities.abilities.Count != 0)
            {
                foreach (BaseAbility ability in entity.Abilities.abilities)
                {
                    output += $"\n{ability.AbilityName}";
                }
            }

            output += "\nActions:";
            foreach (BaseAction action in entity.Actions.actions)
            {
                output += $"\n{action.ActionName}";
            }
            output += $"\nCharacteristics:\nLevel: {entity.Characteristics.CharacterLevel}\nHealth: {entity.Characteristics.HealthBase}\nStamina: {entity.Characteristics.StaminaBase}\nMana:{entity.Characteristics.ManaBase}\nStrength: {entity.Characteristics.StrengthBase}\nDexterity: {entity.Characteristics.DexterityBase}\nVitality: {entity.Characteristics.VitalityBase}\nIntelligence: {entity.Characteristics.IntelligenceBase}\nLuck: {entity.Characteristics.LuckBase}";
            GD.Print(output);

        }

    }
    public void DebugListPlayerCharacters()
    {
        foreach (PlayerEntity entity in playerCharacters.Values)
        {
            string output = $"{entity.ID}\nName: {entity.EntityName}\nDescription: {entity.Description}";

            List<IEffect> effects = entity.Effects.GetActiveEffects();
            output += "\nEffects: ";
            if (effects.Count != 0)
            {
                foreach (IEffect effect in effects)
                {
                    output += $"\n{effect.EffectName}";
                }
            }
            output += "\nAbilities:";
            if (entity.Abilities.abilities.Count != 0)
            {
                foreach (BaseAbility ability in entity.Abilities.abilities)
                {
                    output += $"\n{ability.AbilityName}";
                }
            }

            output += "\nActions:";
            foreach (BaseAction action in entity.Actions.actions)
            {
                output += $"\n{action.ActionName}";
            }
            output += $"\nCharacteristics:\nLevel: {entity.Characteristics.CharacterLevel}\nHealth Base: {entity.Characteristics.HealthBase}; Health Current: {entity.Characteristics.HealthCurrent};\nStamina Base: {entity.Characteristics.StaminaBase}; Stamina Current: {entity.Characteristics.StaminaCurrent}\nMana Base:{entity.Characteristics.ManaBase}; Mana Current: {entity.Characteristics.ManaCurrent}\nStrength: {entity.Characteristics.StrengthBase}; Current:{entity.Characteristics.StrengthCurrent}\nDexterity: {entity.Characteristics.DexterityBase}; Current: {entity.Characteristics.DexterityCurrent}\nVitality: {entity.Characteristics.VitalityBase}; Current: {entity.Characteristics.VitalityCurrent}\nIntelligence: {entity.Characteristics.IntelligenceBase}; Current: {entity.Characteristics.IntelligenceCurrent}\nLuck: {entity.Characteristics.LuckBase}; Current: {entity.Characteristics.LuckCurrent}";
            output += $"\nExperience:\nCharacter Exp: {entity.Experience.CharacterExp}\nStrength Exp: {entity.Experience.StrengthExp}\nDexterity Exp: {entity.Experience.DexterityExp}\nVitality Exp: {entity.Experience.VitalityExp}\nIntelligence Exp:{entity.Experience.IntelligenceExp}\nLuck Exp: {entity.Experience.LuckExp}";
            GD.Print(output);

        }
    }
    public void DebugListWorlds()
    {
        GD.Print("\nWorlds\n");
        foreach(WorldFile world in worlds.Values)
        {
            GD.Print($"{world.ID}\nName: {world.Name}\nSize: {world.Settings.Size}\nSeed: {world.Settings.Seed}");
        }
    }
}

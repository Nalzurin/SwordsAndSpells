using Godot;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

public partial class BiomeLoader : Node
{
    const bool Export = true;
    private const string BiomesFolder = "res://Assets/Biomes/";
    private const string UserBiomesFolder = "user://Assets/Biomes/";
    AssetManager assets;
    TileSet tileSet;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        assets = (AssetManager)GetNode("/root/AssetManager");
		GetBiomes();
        assets.DebugListBiomes();
    }
	void GetBiomes()
    {
        string[] files;
        if (Export)
        {
            files = DirAccess.GetFilesAt(UserBiomesFolder);
        }
        else
        {
            files = DirAccess.GetFilesAt(BiomesFolder);
        }
        foreach(string file in files)
        {
            if (file.EndsWith(".xml"))
            {
                LoadBiome(file);
            }
            
        }
    }
    void LoadBiome(string FilePath)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (Export)
            {
                xmlDoc.Load(ProjectSettings.GlobalizePath(UserBiomesFolder + FilePath));
            }
            else
            {
                xmlDoc.Load(ProjectSettings.GlobalizePath(BiomesFolder + FilePath));
            }

            XmlNodeList biomeNodes = xmlDoc.GetElementsByTagName("BiomeAsset");

            foreach (XmlNode node in biomeNodes)
            {
                Biome biome = ProcessBiome(node);
                assets.AddBiome(biome);
            }
        }
        catch (Exception e)
        {
            GD.PrintErr("Error loading biome from file: " + FilePath);
            GD.PrintErr(e.Message);
        }
    }
	Biome ProcessBiome(XmlNode BiomeNode)
	{
        Biome biome = new Biome();

        XmlNode idNode = BiomeNode.SelectSingleNode("ID");
        if (idNode != null)
            biome.ID = idNode.InnerText;

        XmlNode nameNode = BiomeNode.SelectSingleNode("Name");
        if (nameNode != null)
            biome.Name = nameNode.InnerText;

        XmlNode descNode = BiomeNode.SelectSingleNode("Description");
        if (descNode != null)
            biome.Description = descNode.InnerText;

        XmlNode moistureRangeNode = BiomeNode.SelectSingleNode("MoistureRange");
        if (moistureRangeNode != null)
        {
            int index = 0;
            XmlNodeList moistureValues = moistureRangeNode.SelectNodes("value");
            float[] moistureRange = new float[moistureValues.Count];
            foreach (XmlNode valueNode in moistureValues)
            {
                float value;
                if (float.TryParse(valueNode.InnerText, out value))
                {
                    moistureRange[index] = value;
                    index++;
                }
            }
            biome.MoistureRange = moistureRange;
        }

        XmlNode temperatureRangeNode = BiomeNode.SelectSingleNode("TemperatureRange");
        if (temperatureRangeNode != null)
        {
            XmlNodeList temperatureValues = temperatureRangeNode.SelectNodes("value");
            float[] temperatureRange = new float[temperatureValues.Count];
            int index = 0;
            foreach (XmlNode valueNode in temperatureValues)
            {
                float value;
                if (float.TryParse(valueNode.InnerText, out value))
                {
                    temperatureRange[index] = value;
                    index++;
                }
            }
            biome.TemperatureRange = temperatureRange;
        }

        XmlNode altitudeRangeNode = BiomeNode.SelectSingleNode("AltitudeRange");
        if (altitudeRangeNode != null)
        {
            XmlNodeList altitudeValues = altitudeRangeNode.SelectNodes("value");
            float[] altitudeRange = new float[altitudeValues.Count];
            int index = 0;
            foreach (XmlNode valueNode in altitudeValues)
            {
                float value;
                if (float.TryParse(valueNode.InnerText, out value))
                {
                    altitudeRange[index] = value;
                    index++;
                }
            }
            biome.AltitudeRange = altitudeRange;
        }

        XmlNode priorityNode = BiomeNode.SelectSingleNode("Priority");
        if (priorityNode != null)
        {
            int priority;
            if (int.TryParse(priorityNode.InnerText, out priority))
            {
                biome.Priority = priority;
            }
        }

        XmlNode texturePathNode = BiomeNode.SelectSingleNode("TexturePath");
        if (texturePathNode != null)
        {
            biome.TexturePath = texturePathNode.InnerText;
           
        }
        XmlNode isWalkablePathNode = BiomeNode.SelectSingleNode("IsWalkable");
        if (isWalkablePathNode != null)
        {
            bool value;
            if (bool.TryParse(isWalkablePathNode.InnerText, out value))
            {
                biome.IsWalkable = value;
            }
        }
        return biome;

    }
}

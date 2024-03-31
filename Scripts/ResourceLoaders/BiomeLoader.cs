using Godot;
using System;
using System.IO;
using System.Xml;

public partial class BiomeLoader : Node
{
    private const string BiomesFolder = "Assets/Biomes/";
    AssetManager assets;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        assets = (AssetManager)GetNode("/root/AssetManager");
		GetBiomes();
        assets.DebugListBiomes();
    }
	void GetBiomes()
	{
        string[] files = Directory.GetFiles(BiomesFolder, "*.xml");
        foreach(string file in files)
        {
            LoadBiome(file);
        }
    }
    void LoadBiome(string FilePath)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FilePath);

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
            
            XmlNodeList moistureValues = moistureRangeNode.SelectNodes("value");
            float[] moistureRange = new float[moistureValues.Count];
            foreach (XmlNode valueNode in moistureValues)
            {
                float value;
                int index = 0;
                if (float.TryParse(valueNode.InnerText, out value))
                {
                    moistureRange[index] = value;
                    index++;
                }
            }
        }

        XmlNode temperatureRangeNode = BiomeNode.SelectSingleNode("TemperatureRange");
        if (temperatureRangeNode != null)
        {
            XmlNodeList temperatureValues = temperatureRangeNode.SelectNodes("value");
            float[] temperatureRange = new float[temperatureValues.Count];
            foreach (XmlNode valueNode in temperatureValues)
            {
                float value;
                int index = 0;
                if (float.TryParse(valueNode.InnerText, out value))
                {
                    temperatureRange[index] = value;
                    index++;
                }
            }
        }

        XmlNode priorityNode = BiomeNode.SelectSingleNode("priority");
        if (priorityNode != null)
        {
            int priority;
            if (int.TryParse(priorityNode.InnerText, out priority))
            {
                biome.Priority = priority;
            }
        }

/*        XmlNode tilemapNode = biomeNode.SelectSingleNode("tilemap");
        if (tilemapNode != null)
        {
            biome.Tilemap = tilemapNode.InnerText;
        }*/

        return biome;

    }
}

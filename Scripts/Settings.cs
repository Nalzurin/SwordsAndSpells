using Godot;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public partial class Settings : Node
{
	public WorldGenSettings WorldGenSettings;
	public float GlobalVolume;
    readonly string worldGenPath = "user://Settings/WorldGenSettings.xml";
    public override void _Ready()
	{
		WorldGenSettings = LoadWorldgenSettings();

	}
	private WorldGenSettings LoadWorldgenSettings()
	{

        if (Godot.FileAccess.FileExists(worldGenPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ProjectSettings.GlobalizePath(worldGenPath));
            XmlNodeList list = xmlDoc.GetElementsByTagName("WorldGenSettings");
            if(list.Count == 1)
            {
                XmlNode node = list[0];
                int size; int seed; float frequency; int octaves; float lacunarity; int minBiomeSize;
                size = int.Parse(node.SelectSingleNode("Size")?.InnerText ?? "256");
                seed = int.Parse(node.SelectSingleNode("Seed")?.InnerText ?? "0");
                frequency = float.Parse(node.SelectSingleNode("Frequency")?.InnerText ?? "0.01");
                lacunarity = float.Parse(node.SelectSingleNode("Lacunarity")?.InnerText ?? "1");
                octaves = int.Parse(node.SelectSingleNode("Octaves")?.InnerText ?? "4");
                minBiomeSize = int.Parse(node.SelectSingleNode("MinimumBiomeSize")?.InnerText ?? "25");
                return new WorldGenSettings(size, seed, frequency, octaves, lacunarity, minBiomeSize);
            }
            
        }
        return new WorldGenSettings();
    }
	private void SaveWorldgetSettings()
	{
        XmlSerializer serializer = new XmlSerializer(typeof(WorldGenSettings));
        // Create a StreamWriter to write to the specified file
        using (StreamWriter writer = new StreamWriter(ProjectSettings.GlobalizePath(worldGenPath)))
        {
            // Serialize the settings object to the file
            serializer.Serialize(writer, WorldGenSettings);
        }
    }
}

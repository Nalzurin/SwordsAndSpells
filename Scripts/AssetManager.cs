using Godot;
using System;
using System.Collections.Generic;

public partial class AssetManager : Node
{
    Dictionary<string, Biome> biomes = new Dictionary<string, Biome>();

    public void AddBiome(Biome biome)
    {
        biomes.Add(biome.ID, biome);
    }
    public Dictionary<string, Biome> GetBiomes()
    {
        return biomes;
    }
    public Biome GetBiome(string id)
    {
        if(biomes.ContainsKey(id)) return biomes[id];
        return null;
    }
    public void DebugListBiomes()
    {
        foreach(Biome biome in biomes.Values)
        {
            GD.Print($"{biome.ID}\nName: {biome.Name}\nDescription: {biome.Description}\nMoisture Range:[{biome.MoistureRange[0]};{biome.MoistureRange[1]}]\nTemperature Range:[{biome.TemperatureRange[0]};{biome.TemperatureRange[1]}]");
        }
    }

}

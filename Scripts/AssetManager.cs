using Godot;
using System;
using System.Collections.Generic;

public partial class AssetManager : Node
{
    Dictionary<string, Biome> biomes = new Dictionary<string, Biome>();
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

    public void AddBiome(Biome biome)
    {
        biomes.Add(biome.ID, biome);
    }
    public void SetBiomeTileSetSourceId(string BiomeID, int SourceId)
    {
        biomes[BiomeID].TilesetSourceId = SourceId;
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
            GD.Print($"{biome.ID}\nName: {biome.Name}\nDescription: {biome.Description}\nMoisture Range:[{biome.MoistureRange[0]};{biome.MoistureRange[1]}]\nTemperature Range:[{biome.TemperatureRange[0]};{biome.TemperatureRange[1]}]\nAltitude Range:[{biome.AltitudeRange[0]};{biome.AltitudeRange[1]}]\nPriority: {biome.Priority}\nTilesetSourceID: {biome.TilesetSourceId}");
        }
    }

}

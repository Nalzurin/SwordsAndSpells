using Godot;
using System;

public class Biome
{
    public string ID { get; set; }
    public string Name {  get; set; }
    public string Description { get; set; }
    public float[] MoistureRange { get; set; }
    public float[] TemperatureRange { get; set; }
    public float[] AltitudeRange { get; set; }

    public int Priority { get; set; }
    public string TexturePath { get; set; }
    public int TilesetSourceId { get; set; }
    public bool IsWalkable { get; set; }

    public Biome() : this("temperateforest", "Temperate Forest", "A temperate forest is a biome with moderate temperatures and seasonal changes, featuring diverse trees like deciduous and coniferous species. Found in areas with four distinct seasons, these forests are rich in plant and animal life.", new float[] { 0.4f, 0.5f }, new float[] { 0.4f, 0.5f }, new float[] { 0.4f, 0.5f }, 1, "Assets/Sprites/Biomes/TemperateForest.png", true) { }
    public Biome(string _ID, string _Name, string _Description, float[] _MoistureRange, float[] _TemperatureRange, float[] _AltitudeRange, int _Priority, string _TexturePath, bool _IsWalkable )
    {
        ID = _ID;
        Name = _Name;
        Description = _Description;
        MoistureRange = new float[] { _MoistureRange[0], _MoistureRange[1] };
        TemperatureRange = new float[] { _TemperatureRange[0], _TemperatureRange[1] };
        AltitudeRange = new float[] { _AltitudeRange[0], _AltitudeRange[1] };
        Priority = _Priority;
        TexturePath = _TexturePath;
        IsWalkable = _IsWalkable;
    }

}
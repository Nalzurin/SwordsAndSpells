using Godot;
using System.Xml.Serialization;

[XmlRoot("WorldGenSettings")]
public partial class WorldGenSettings : Resource
{
    [XmlElement("Size")]
    [Export]
    public int Size { get; set; }
    [XmlElement("Seed")]
    [Export]
    public int Seed { get; set; }
    [XmlElement("Frequency")]
    [Export]
    public float Frequency { get; set; }
    [XmlElement("Octaves")]
    [Export]
    public int Octaves { get; set; }
    [XmlElement("Lacunarity")]
    [Export]
    public float Lacunarity { get; set; }
    [XmlElement("MinimumBiomeSize")]
    [Export]
    public int MinBiomeSize { get; set; }

    public WorldGenSettings() : this(512, 0, 0.001f, 4, 2f, 50) { }
    public WorldGenSettings(int _Size, int _Seed, float _Frequency, int _Octaves, float _Lacunarity, int _MinBiomeSize )
    {
        Size = _Size;
        Seed = _Seed;
        Octaves = _Octaves;
        Lacunarity = _Lacunarity;
        Frequency = _Frequency;
        MinBiomeSize = _MinBiomeSize;
    }
}
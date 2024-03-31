using Godot;

public partial class WorldGenSettings : Resource
{
    [Export]
    public int Size { get; set; }
    [Export]
    public int Seed { get; set; }
    [Export]
    public float Frequency { get; set; }
    [Export]
    public int Octaves { get; set; }
    [Export]
    public float Lacunarity { get; set; }
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
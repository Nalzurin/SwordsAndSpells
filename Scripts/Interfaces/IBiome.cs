using Godot;
using System;

public interface IBiome
{
    string Name { get; }
    string Description { get; }
    float[] MoistureRange {  get; }
    float[] TemperatureRange { get; }
    int Priority { get; }
}

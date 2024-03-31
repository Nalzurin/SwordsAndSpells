using Godot;
using System;

public interface IAsset
{
    string ID { get; set; }
    string AssetName { get; set; }
    string Description { get; set; }
    string SpritePath { get; set; }
}

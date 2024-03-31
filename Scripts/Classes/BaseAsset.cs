using Godot;
using System;

public abstract partial class BaseAsset : Node, IAsset
{
    // IAsset properties
    public string ID { get; set; }
    public string AssetName { get; set; }
    public string Description { get; set; }
    public string SpritePath { get; set; }

}
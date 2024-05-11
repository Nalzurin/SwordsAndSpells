using Godot;
using System;

public abstract partial class BaseItem : Node
{
    public string ID { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string Rarity { get; set; }
    public int ItemLevel { get; set; }
    public string SpritePath { get; set; }
}
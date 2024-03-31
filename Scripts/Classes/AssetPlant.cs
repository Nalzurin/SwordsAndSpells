using Godot;
using System;

public partial class AssetPlant: BaseAsset
{
    AssetPlant() : this ("AssetPlant_SpruceTree", "Spruce Tree", "A spruce tree is a tall evergreen tree with needle-like leaves that stay green all year round. It has a conical shape with branches that point upwards. The bark is usually rough and grayish-brown in color. Spruce trees are common in colder climates and are often used for timber and Christmas trees.", "/Assets/Sprites/Plants/Trees/Tree_Spruce") { }    
    AssetPlant(string _ID, string _AssetName, string _Description, string _SpritePath)
    {
        ID = _ID;
        AssetName = _AssetName;
        Description = _Description;
        SpritePath = _SpritePath;
    }


}
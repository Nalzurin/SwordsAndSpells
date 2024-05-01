using Godot;
using System;

public partial class TilesetGenerator : Node
{
    AssetManager assets;
    private int tileSize = 16;
    private TileSet tileset;

    public override void _Ready()
    {
        assets = (AssetManager)GetNode("/root/AssetManager");
        tileset = new TileSet();
        tileset.TileSize = new Vector2I(tileSize, tileSize);
        // Loop through the biomes and add tiles to the tileset
        foreach (var biome in assets.GetBiomes())
        {
            AddAtlas(biome.Value.TexturePath, biome.Key);
        }
    }

    // Add a tile to the tileset
    private void AddAtlas(string texturePath, string name)
    {
        // Load texture
        Texture texture = GD.Load<Texture>(texturePath);

        TileSetAtlasSource TileSource = new TileSetAtlasSource();
        TileSource.Texture = (Texture2D)texture;
        TileSource.CreateTile(Vector2I.Zero);

        // Add the AtlasTexture to the tileset with the unique string ID
        int id = tileset.AddSource(TileSource);
        assets.SetBiomeTileSetSourceId(name, id);
    }
}

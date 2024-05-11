using Godot;
using System;
using System.Collections.Generic;

public class WorldFile
{
    public string ID { get; set; }
    public string Name { get; set; }
    public Vector2I WorldSpawn { get; set; }
    public WorldGenSettings Settings { get; set; }
    public Dictionary<Vector2I, BaseEntity> SavedEntities { get; set; }
    public WorldFile() : this("DummyId", "DummyName", Vector2I.Zero, new WorldGenSettings(), new Dictionary<Vector2I, BaseEntity>()) { }
    public WorldFile(string _id, string _name, Vector2I _worldSpawn, WorldGenSettings _settings, Dictionary<Vector2I, BaseEntity> _entities)
    {
        ID = _id;
        Name = _name;
        Settings = _settings;
        SavedEntities = _entities;
        WorldSpawn = _worldSpawn;

    }

}

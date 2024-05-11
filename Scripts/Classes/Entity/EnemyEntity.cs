using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public partial class EnemyEntity : BaseEntity
{
    public string Rarity { get; set; }
    public List<string> BiomeSpawns { get; set; }
    public EnemyEntity() : this("DummyEntityID", "DummyEntity", "DummyDescription", "Assets/Sprites/Items/Consumables/Food/Lemon.png", "Dummy", "Legendary",new List<string> { "Biome_Desert"}, new Effects(),  new Abilities(), new Actions(), new Characteristics())
    {
        Effects.SetParent(this);
        Characteristics.SetParent(this);
        Abilities.SetParent(this);
        Actions.SetParent(this);
    }
    public EnemyEntity(string _ID, string _EntityName, string _Description, string _SpritePath, string _TargetType, string _Rarity, List<string> _BiomeSpawns, Effects _Effects, Abilities _Abilities, Actions actions, Characteristics _Characteristics)
    {
        ID = _ID;
        EntityName = _EntityName;
        Description = _Description;
        SpritePath = _SpritePath;
        TargetType = _TargetType;
        Rarity = _Rarity;
        BiomeSpawns = _BiomeSpawns;
        Effects = _Effects;
        Abilities = _Abilities;
        Actions = actions;
        Characteristics = _Characteristics;
        Effects.SetParent(this);
        Characteristics.SetParent(this);
        Abilities.SetParent(this);
        Actions.SetParent(this);
    }


}
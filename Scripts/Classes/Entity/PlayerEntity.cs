using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public partial class PlayerEntity : BaseEntity
{

    public Experience Experience { get; set; }
    public Inventory Inventory { get; set; }
    public string PlayerImagePath { get; set; }
    public ImageTexture GetImage()
    {
        ImageTexture image = GD.Load<ImageTexture>(PlayerImagePath);
        return image;
    }
    public PlayerEntity() : this("DummyEntityID", "DummyEntity", "DummyDescription", "Assets/Sprites/Items/Consumables/Food/Lemon.png", "Assets/Sprites/Items/Consumables/Food/Lemon.png", "Dummy", new Effects(), new Abilities(), new Actions(), new Characteristics(), new Experience(), new Inventory())
    {
        Effects.SetParent(this);
        Characteristics.SetParent(this);
        Abilities.SetParent(this);
        Actions.SetParent(this);
        Experience.SetParent(this);
    }
    public PlayerEntity(string _ID, string _EntityName, string _Description, string _PlayerImagePath, string _SpritePath, string _TargetType, Effects _Effects, Abilities _Abilities, Actions actions, Characteristics _Characteristics, Experience _Experience, Inventory inventory)
    {
        ID = _ID;
        EntityName = _EntityName;
        Description = _Description;
        PlayerImagePath = _PlayerImagePath;
        SpritePath = _SpritePath;
        TargetType = _TargetType;
        Effects = _Effects;
        Abilities = _Abilities;
        Actions = actions;
        Characteristics = _Characteristics;
        Experience = _Experience;
        Effects.SetParent(this);
        Characteristics.SetParent(this);
        Abilities.SetParent(this);
        Actions.SetParent(this);
        Inventory = inventory;
    }


}
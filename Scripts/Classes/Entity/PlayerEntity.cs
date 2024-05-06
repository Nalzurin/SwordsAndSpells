using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public partial class PlayerEntity : BaseEntity
{

    [Export]
    public Sprite2D PlayerSprite;
    Experience Experience { get; set; }
    public override void _Ready()
    {
        PlayerSprite.Texture = GetSprite();
        GD.Print(Characteristics.HealthBase.ToString());
    }
    void DebugDamageHealth()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    public PlayerEntity() : this("DummyEntityID", "DummyEntity", "DummyDescription", "Assets/Sprites/Items/Consumables/Food/Lemon.png", "Dummy", new Effects(), new Abilities(), new Characteristics(), new Experience())
    {
        Effects.SetParent(this);
        Characteristics.SetParent(this);
    }
    public PlayerEntity(string _ID, string _EntityName, string _Description, string _SpritePath, string _TargetType, Effects _Effects, Abilities _Abilities, Characteristics _Characteristics, Experience _Experience)
    {
        ID = _ID;
        EntityName = _EntityName;
        Description = _Description;
        SpritePath = _SpritePath;
        TargetType = _TargetType;
        Effects = _Effects;
        Abilities = _Abilities;
        Characteristics = _Characteristics;
        Experience = _Experience;
    }


}
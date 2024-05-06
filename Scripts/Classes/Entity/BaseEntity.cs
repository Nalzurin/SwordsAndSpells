using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

public abstract partial class BaseEntity : Node, ITargetable
{
    public string ID { get; set; }
    public string EntityName { get; set; }
    public string Description { get; set; }
    public string SpritePath { get; set; } 
    public string TargetType {  get; set; }
    public Effects Effects {  get; set; }
    public Abilities Abilities { get; set; }
    public Actions Actions {  get; set; }
    public Characteristics Characteristics { get; set; }

    public Texture2D GetSprite()
    {
       return GD.Load<Texture2D>(SpritePath);

    }
    public BaseEntity():this("DummyEntityID", "DummyEntity", "DummyDescription", "Assets/Sprites/Items/Consumables/Food/Lemon.png", "Dummy", new Effects(), new Abilities(), new Actions(), new Characteristics()) { }
    public BaseEntity(string iD, string entityName, string description, string spritePath, string targetType, Effects effects, Abilities abilities, Actions actions, Characteristics characteristics)
    {
        ID = iD;
        EntityName = entityName;
        Description = description;
        SpritePath = spritePath;
        TargetType = targetType;
        Effects = effects;
        Abilities = abilities;
        Actions = actions;
        Characteristics = characteristics;
    }
}
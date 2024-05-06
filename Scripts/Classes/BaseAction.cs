using Godot;
using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

public class BaseAction
{
   
    public string ID { get; set; }
    public string ActionName { get; set; }
    public string Description { get; set; }
    public string SpritePath { get; set; }
    public string TargetType {  get; set; }
    public List<IEffect> Effects { get; set; }

    public void Do_Action(ITargetable target)
    {
        if(IsTargetCorrect(target))
        {
            foreach(IEffect effect in Effects)
            {
                target.Effects.AddEffect(effect);
            }
            
        }
    }
    private bool IsTargetCorrect(ITargetable target)
    {
        if (TargetType == "Self" && target.TargetType != "Player")
        {
            GD.Print("Wrong Target(Not Self)");
            return false;
        }
        if (TargetType == "Enemy" && target.TargetType != "Enemy")
        {
            GD.Print("Wrong Target(Not Enemy)");
            return false;
        }
        if (TargetType == "Ally" && target.TargetType != "Ally")
        {
            GD.Print("Wrong Target(Not Ally)");
            return false;
        }
        return true;
    }
    public BaseAction(): this("DummyActionID", "Dummy Action", "If you see this action, something went wrong", "Assets/Sprites/Items/Consumables/Food/Lemon.png", "Dummy", new List<IEffect> {new ChangeHealthEffect() } ) { }
    public BaseAction(string iD, string actionName, string description, string spritePath, string targetType, List<IEffect> effects)
    {
        ID = iD;
        ActionName = actionName;
        Description = description;
        SpritePath = spritePath;
        TargetType = targetType;
        Effects = effects;
    }
}
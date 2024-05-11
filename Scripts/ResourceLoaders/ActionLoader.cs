using Godot;
using System;
using System.Collections.Generic;
using System.Xml;

public class ActionLoader
{
    XmlNode actionNode { get; set; }
    public ActionLoader(XmlNode _actionNode) { actionNode = _actionNode; }
    public BaseAction GenerateAction()
    {
        string id = actionNode.SelectSingleNode("ID")?.InnerText ?? string.Empty;
        string name = actionNode.SelectSingleNode("Name")?.InnerText ?? string.Empty;
        string description = actionNode.SelectSingleNode("Description")?.InnerText ?? string.Empty;
        string spritePath = actionNode.SelectSingleNode("SpritePath")?.InnerText ?? string.Empty;
        string targetType = actionNode.SelectSingleNode("TargetType")?.InnerText ?? string.Empty;
        Dictionary<string, int> cost = new Dictionary<string, int>();
        XmlNode costNode = actionNode.SelectSingleNode("Cost");
        XmlNode value = costNode.SelectSingleNode("Stamina");
        if(value != null) cost.Add("Stamina", int.Parse(value.InnerText));
        value = costNode.SelectSingleNode("Mana");
        if (value != null) cost.Add("Mana", int.Parse(value.InnerText));
        value = costNode.SelectSingleNode("Health");
        if (value != null) cost.Add("Health", int.Parse(value.InnerText));
        List<IEffect> effects = new List<IEffect>();
        XmlNodeList effectNodes = actionNode.SelectSingleNode("Effects").SelectNodes("Effect");
        foreach (XmlNode effectNode in effectNodes)
        {
            EffectLoader loader = new EffectLoader(effectNode, name);
            effects.Add(loader.GenerateEffect());
        }
        return new BaseAction(id, name, description, "Assets/Sprites/"+spritePath, targetType, cost, effects);

    }
}

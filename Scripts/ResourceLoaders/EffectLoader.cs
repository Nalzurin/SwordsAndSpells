using Godot;
using System;
using System.Xml;

public class EffectLoader
{
    XmlNode effectNode {  get; set; }
    string Source { get; set; }
    public EffectLoader(XmlNode _effectNode, string _Source) { effectNode = _effectNode;  Source = _Source;}
    public IEffect GenerateEffect()
    {
        string id = effectNode.SelectSingleNode("ID")?.InnerText ?? string.Empty;
        string name = effectNode.SelectSingleNode("Name")?.InnerText ?? string.Empty;
        string effectTypeClass = effectNode.SelectSingleNode("EffectTypeClass")?.InnerText ?? string.Empty;
        string description = effectNode.SelectSingleNode("Description")?.InnerText ?? string.Empty;
        int duration = int.Parse(effectNode.SelectSingleNode("Duration")?.InnerText ?? "0");
        int value = int.Parse(effectNode.SelectSingleNode("Value")?.InnerText ?? "0");
        string effectType = effectNode.SelectSingleNode("EffectType")?.InnerText ?? string.Empty;
        bool isHarmful = bool.Parse(effectNode.SelectSingleNode("IsHarmful")?.InnerText ?? "false");
        switch (effectTypeClass)
        {
            case "ChangeHealthEffect":
                return new ChangeHealthEffect(id, name, description, duration, value, effectType, isHarmful, Source);
            case "ChangeManaEffect":
                return new ChangeManaEffect(id, name, description, duration, value, effectType, isHarmful, Source);
            case "ChangeStaminaEffect":
                return new ChangeStaminaEffect(id, name, description, duration, value, effectType, isHarmful, Source);
            case "ChangeStrengthEffect":
                return new ChangeStrengthEffect(id, name, description, duration, value, effectType, isHarmful, Source);
            case "ChangeDexterityEffect":
                return new ChangeStrengthEffect(id, name, description, duration, value, effectType, isHarmful, Source);
            case "ChangeVitalityEffect":
                return new ChangeStrengthEffect(id, name, description, duration, value, effectType, isHarmful, Source);
            case "ChangeIntelligenceEffect":
                return new ChangeStrengthEffect(id, name, description, duration, value, effectType, isHarmful, Source);
            case "ChangeLuckEffect":
                return new ChangeStrengthEffect(id, name, description, duration, value, effectType, isHarmful, Source);

            default:
                throw new InvalidOperationException($"Unknown EffectTypeClass: {effectTypeClass}");
        }
    }
}

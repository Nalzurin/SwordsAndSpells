using Godot;
using System;
using System.Collections.Generic;
using System.Xml;

public class Effects
{
    private BaseEntity parent;
    private List<IEffect> activeEffects {  get; set; }
    public List<IEffect> GetActiveEffects() { return activeEffects; }
    public void AddEffect(IEffect effect)
    {
        effect.Apply(parent);
        if (effect.Duration > 0)
        {
            activeEffects.Add(effect);
        }
        
        
    }
    public void UpdateEffects()
    {
        activeEffects.RemoveAll(effect => !effect.Update(parent));
    }
    public void RemoveEffect(IEffect effect)
    {
        if (activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
        }
    }
    public void SetParent(BaseEntity _parent)
    {
        parent = _parent;
    }
    public void WriteEffects(XmlWriter writer, Effects effects)
    {
        // Start the "Effects" element
        writer.WriteStartElement("Effects");

        // Iterate through the list of effects
        foreach (var effect in effects.GetActiveEffects())
        {
            // Start the "Effect" element
            writer.WriteStartElement("Effect");

            // Write the ID, Name, EffectTypeClass, Description, Duration, Value, EffectType, and IsHarmful
            WriteElement(writer, "ID", effect.ID);
            WriteElement(writer, "Name", effect.EffectName);
            WriteElement(writer, "EffectTypeClass", effect.GetType().Name);
            WriteElement(writer, "Description", effect.Description);
            WriteElement(writer, "Duration", effect.Duration.ToString());
            // Depending on your effect implementation, you may need to get additional properties here
            WriteElement(writer, "IsHarmful", effect.IsHarmful.ToString());

            // End the "Effect" element
            writer.WriteEndElement();
        }

        // End the "Effects" element
        writer.WriteEndElement();
    }
    private void WriteElement(XmlWriter writer, string elementName, string value)
    {
        writer.WriteStartElement(elementName);
        writer.WriteString(value);
        writer.WriteEndElement();
    }
    public Effects() : this(new List<IEffect>(),null) { }
    public Effects(List<IEffect> _activeEffects, BaseEntity _parent)
    {
        activeEffects = _activeEffects;
        parent = _parent;


    }
}

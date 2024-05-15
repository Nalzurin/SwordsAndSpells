using Godot;
using System;
using System.Collections.Generic;
using System.Xml;

public class Actions
{
    private BaseEntity parent {  get; set; }
    public List<BaseAction> actions { get; private set; }
    public void SetParent(BaseEntity parent)
    {
        this.parent = parent;
    }
    public void ClearActions()
    {
        actions = new List<BaseAction>();
    }
    public void AddAction(BaseAction action)
    {
        actions.Add(action);
    }
    public void RemoveAction(BaseAction action)
    {
        actions.Remove(action);
    }
    public void RemoveAction(int  index)
    {
        actions.RemoveAt(index);
    }
    public void WriteActions(XmlWriter writer, Actions actions)
    {
        // Start the "Actions" element
        writer.WriteStartElement("Actions");

        // Iterate through the list of actions
        foreach (var action in actions.actions)
        {
            // Start the "Action" element
            writer.WriteStartElement("Action");

            // Write the ID, Name, Description, SpritePath, and TargetType
            WriteElement(writer, "ID", action.ID);
            WriteElement(writer, "Name", action.ActionName);
            WriteElement(writer, "Description", action.Description);
            WriteElement(writer, "SpritePath", action.SpritePath);
            WriteElement(writer, "TargetType", action.TargetType);

            // Write the Cost dictionary
            writer.WriteStartElement("Cost");
            foreach (var costEntry in action.Cost)
            {
                WriteElement(writer, costEntry.Key, costEntry.Value.ToString());
            }
            writer.WriteEndElement(); // End the "Cost" element

            // Write the effects associated with the action
            parent.Effects.WriteEffects(writer, new Effects(action.Effects, null));

            // End the "Action" element
            writer.WriteEndElement();
        }

        // End the "Actions" element
        writer.WriteEndElement();
    }
    private void WriteElement(XmlWriter writer, string elementName, string value)
    {
        writer.WriteStartElement(elementName);
        writer.WriteString(value);
        writer.WriteEndElement();
    }
    public Actions() : this(new List<BaseAction>(), null) { }
    public Actions(List<BaseAction> _Actions, BaseEntity _Parent)
    {
        actions = _Actions;
        parent = _Parent;
    }
}

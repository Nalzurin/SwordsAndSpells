using Godot;
using System;
using System.Collections.Generic;

public interface IInventory
{
    int size { get; }
    List<IItem> items { get; }

}

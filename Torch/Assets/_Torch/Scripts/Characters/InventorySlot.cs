using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
{
    // -- PROPERTY

    public bool IsEmpty => Count == 0;

    // -- FIELDS

    public Item Item;
    public int Count;

    // -- METHODS

    public bool IsItem(Item item)
    {
        return Item != null && item.GetInstanceID() == Item.GetInstanceID();
    }

    public bool CanAddItem(Item item)
    {
        return Item == null || Count < Item.MaximumStacks && IsItem(item);
    }

    public void AddItem(Item item)
    {
        Item = item;
        ++Count;
    }
}

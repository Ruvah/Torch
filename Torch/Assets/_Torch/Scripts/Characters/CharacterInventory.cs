using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    // -- TYPES

    // -- FIELDS

    private const int InventorySpace = 28;

    private InventorySlot[] itemSlots = new InventorySlot[InventorySpace];

    // -- METHODS

    public void AddItem(Item item)
    {
        var slot = itemSlots.First(x => x.CanAddItem(item));
        slot.AddItem(item);
    }

    private bool GetFirstEmptySlot(out InventorySlot inventory_slot)
    {
        inventory_slot = itemSlots.First(x => x.IsEmpty);
        return inventory_slot != null;
    }

    private int GetEmptySlots(InventorySlot[] results, int amount_needed)
    {
        int found = 0;
        foreach (var slot in itemSlots)
        {
            if (!slot.IsEmpty)
            {
                continue;
            }

            results[found] = slot;
            ++found;
            if (found == amount_needed)
            {
                break;
            }
        }

        return found;
    }

    // -- UNITY

    private void Awake()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = new InventorySlot();
        }
    }
}

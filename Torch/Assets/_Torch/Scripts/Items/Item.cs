using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    // -- PROPERTIES

    public int MaximumStacks => maximumStacks;
    public bool IsStackable => maximumStacks > 0;

    // -- FIELDS

    [SerializeField] private string DisplayName;

    [Tooltip("0 means that this item can not stack in the inventory")]
    [SerializeField] private int maximumStacks;

}

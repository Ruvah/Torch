using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIVerticalTable : UIObject
{
    public enum VerticalTableAlignment
    {
        Centre,
        Left,
        Right,
        Count
    }
    
    // -- FIELDS


    public float Spacing
    {
        get => _Spacing;
        set
        {
            _Spacing = value;
            RecreateTable();
        }
    }

    private float _Spacing;
    private List<KeyValuePair<UIObject, VerticalTableAlignment>> ItemsList;

    [SerializeField] private VerticalTableAlignment ItemAlignment;

    // -- METHODS

    public void AddItem(UIObject item)
    {
        AddItem(item, ItemAlignment);
    }

    public virtual void AddItem(UIObject item, VerticalTableAlignment alignment)
    {
        var last_item = ItemsList.Last();
        Vector2 anchored_position = ContentRect.max;
    }
    
    private void RecreateTable()
    {
        
    }
    
    // -- UNITY
}

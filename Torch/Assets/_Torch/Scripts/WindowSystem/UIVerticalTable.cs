using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIVerticalTable : UIObject
{
    public enum HorizontalAlignment
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
    
    protected List<KeyValuePair<UIObject, HorizontalAlignment>> ItemsList = new List<KeyValuePair<UIObject, HorizontalAlignment>>();

    [SerializeField]
    private float _Spacing;

    // -- METHODS

    public virtual void AddItem(UIObject item, HorizontalAlignment alignment)
    {
        var anchored_position = ContentRect.max - item.ContentRect.size * 0.5f;
        anchored_position.y -= Spacing;


        if (ItemsList.Count > 0)
        {
            foreach (var item_aligment in ItemsList)
            {
                anchored_position.y -= item_aligment.Key.ContentRect.height;
                anchored_position.y -= Spacing;
            }
        }

        switch (alignment)
        {
            case HorizontalAlignment.Left:
            {
                anchored_position.x = (-ContentRect.width + item.ContentRect.width) * 0.5f;
                break;
            }

            case HorizontalAlignment.Centre:
            {
                anchored_position.x = 0;   
                break;
            }

            case HorizontalAlignment.Right:
            {
                anchored_position.x = (ContentRect.width - item.ContentRect.width) * 0.5f;
                break;
            }
        }

        item.RectTransform.anchoredPosition = anchored_position;

        ItemsList.Add(new KeyValuePair<UIObject, HorizontalAlignment>(item, alignment));
    }
    
    private void RecreateTable()
    {
        
    }
    
    // -- UNITY
}

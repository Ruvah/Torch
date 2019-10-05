using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    // -- FIELDS
    [ReadOnlyInInspector]
    public UIObject Parent;
    
    protected Rect ContentRect => ((RectTransform) transform).rect;

    
    // -- METHODS


    protected virtual void OnValidate()
    {
        foreach (Transform immediate_child in transform)
        {
            var ui_object = immediate_child.GetComponent<UIObject>();
            ui_object.Parent = this;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    // -- FIELDS
    [ReadOnlyInInspector]
    public UIObject Parent;
    
    public Rect ContentRect => _RectTransform.rect;

    public RectTransform RectTransform => _RectTransform;

    [HideInInspector, SerializeField]
    private RectTransform _RectTransform;
    
    // -- METHODS


    protected virtual void OnValidate()
    {
        _RectTransform = (RectTransform) transform;

        foreach (Transform immediate_child in transform)
        {
            var ui_object = immediate_child.GetComponent<UIObject>();
            if (ui_object != null)
            {
                ui_object.Parent = this;
            }
        }
    }
    
    // -- UNITY


}

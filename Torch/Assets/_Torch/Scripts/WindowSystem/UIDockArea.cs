using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIDockArea : UIObject
{
    // -- FIELDS
    
    
    public UIContent Content;

    private RectTransform ContentTransform;
    private RectTransform RectTransform;
    
    // -- METHODS
    
    
    // -- UNITY


    private void Update()
    {
        if (Content)
        {
            ContentTransform.sizeDelta = RectTransform.rect.size;
            ContentTransform.anchoredPosition = Vector2.zero;
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if (Content)
        {
            RectTransform = transform as RectTransform;
            ContentTransform = Content.transform as RectTransform;
        }
    }
}

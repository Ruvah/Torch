using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIDockArea : MonoBehaviour
{
    // -- FIELDS
    
    
    public UIContent Content;

    private RectTransform ContentTransform;
    private RectTransform RectTransform;
    
    // -- METHODS
    
    
    // -- UNITY


    private void Update()
    {
        ContentTransform.sizeDelta = RectTransform.rect.size;
        ContentTransform.anchoredPosition = Vector2.zero;
    }

    private void Awake()
    {
        RectTransform = transform as RectTransform;
        ContentTransform = Content.transform as RectTransform;
    }
}

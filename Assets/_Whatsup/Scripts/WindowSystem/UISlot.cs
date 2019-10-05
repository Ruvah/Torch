using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    // -- PUBLIC

    public delegate void UISlotDropped(UISlot slot, Rect dropped_area);
    public event UISlotDropped OnDrop;

    public int Index;
    public RectTransform RectTransform => _RectTransform;
    public ISlottable Slottable
    {
        get => _Slottable;
        set
        {
            _Slottable = value;
            OnNewSlottable();
        }
    }
    
    public void Resize(Vector2 size)
    {
        _RectTransform.sizeDelta = size;
    }
    
    public void OnMouseDrag()
    {
        if (AllowDragging && _Slottable != null)
        {
            IsBeingDragged = true;
            Foreground.transform.position = Input.mousePosition;
        }
    }

    public void OnMouseDrop()
    {
        if (IsBeingDragged)
        {
            var rect_transform = Foreground.rectTransform;
            Rect drop_rect = new Rect(rect_transform.position, _RectTransform.sizeDelta);
            OnDrop?.Invoke(this, drop_rect);
            Foreground.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
    
    // -- PROTECTED
    

    
    // -- PRIVATE


    private bool IsBeingDragged;
    private ISlottable _Slottable;
    private RectTransform _RectTransform;
    
    [SerializeField] private Image Background;
    [SerializeField] private Image Foreground;
    [SerializeField] private bool AllowDragging;

    private void OnNewSlottable()
    {
        if (_Slottable == null)
        {
            var color = Foreground.color;
            color.a = 0;
            Foreground.color = color;
        }
        else
        {
            var color = Foreground.color;
            color.a = 1;
            Foreground.color = color;
            Foreground.sprite = _Slottable.Icon;
        }
    }
    
    
    // -- UNITY


    private void Awake()
    {
        _RectTransform = (RectTransform) transform;
    }
}

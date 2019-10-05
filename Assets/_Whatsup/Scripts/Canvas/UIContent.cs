using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContent : MonoBehaviour
{
    // -- PUBLIC

    public Vector2 MinimumSize => _MinimumSize;
    
    
    // -- PROTECTED
    
    protected Rect ContentRect => ((RectTransform) transform).rect;
    protected Vector2 _MinimumSize;
}

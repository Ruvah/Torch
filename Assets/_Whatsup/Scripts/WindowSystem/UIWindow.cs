using UnityEngine;

[ExecuteInEditMode]
public class UIWindow : UIObject
{
    // -- PUBLIC

    
    public Vector2 Margins
    {
        get
        {
            return _Margins;
            
        }
        set
        {
            _Margins = value;
            CalculateCenter();
        }
    }
    
    // -- PRIVATE


    private RectTransform RectTransform;

    [SerializeField] private RectTransform CenterArea;
    [SerializeField] private Vector2 _Margins;

    private void CalculateCenter()
    {
        if (CenterArea == null)
        {
            return;
        }

        CenterArea.sizeDelta = RectTransform.rect.size - Margins;
    }
    
    
    // -- UNITY


    private void Start()
    {
        RectTransform = (RectTransform) transform;
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            CalculateCenter();
        }
        #endif
    }
}

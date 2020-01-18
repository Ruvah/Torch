using System.Collections.Generic;
using UnityEngine;

public abstract class UISlotContent : UIContent
{
    // -- PUBLIC


    // -- PROTECTED

    
    protected int TotalSlots;
    protected UISlot[] Slots;
    protected Rect[] SlotRectsArray;

    protected abstract void Initialize();

    protected abstract ISlottable GetISlottableFromSource(int index);

    protected abstract void UpdateSlots(IEnumerable<int> indices);

    protected abstract void UISlot_OnMouseDrop(UISlot slot, Rect dropped_area);

    // -- PRIVATE


    [SerializeField] private Vector2 SlotDimensions;
    [SerializeField] private Vector2Int MinSlotColsRows;
    [SerializeField] private Vector2 Spacing;
    [SerializeField] private GameObject SlotPrefab;
    
    private Vector2Int CurrentColsRows;

    private void CreateSlot(Vector2 position, int index)
    {
        var slot = Instantiate(SlotPrefab, transform).GetComponent<UISlot>();
        RectTransform slot_transform = (RectTransform) slot.transform;
        slot_transform.anchoredPosition = position;
        slot.Index = index;
        slot.Resize(SlotDimensions);
        slot.Slottable = GetISlottableFromSource(index);
        slot.OnDrop += UISlot_OnMouseDrop;
        Slots[index] = slot;
        SlotRectsArray[index] = new Rect(slot_transform.position,slot_transform.sizeDelta);
    }

    private void CreateSlots()
    {
        var content_rect = ContentRect;
        Slots = new UISlot[TotalSlots];
        SlotRectsArray = new Rect[TotalSlots];

        Vector2 space_per_slot = Spacing + SlotDimensions;
        
        Vector2 start_pos = new Vector2
            (
            content_rect.xMin + Spacing.x + SlotDimensions.x * 0.5f,
            content_rect.yMax - Spacing.y - SlotDimensions.y * 0.5f
            );

        Vector2 unmargined_max_pos = new Vector2
        (
            content_rect.xMax - SlotDimensions.x * 0.5f,
            content_rect.yMin + SlotDimensions.y * 0.5f
        );

        CurrentColsRows.x = Mathf.Max(MinSlotColsRows.x ,(int) (unmargined_max_pos.x / space_per_slot.x));
        CurrentColsRows.y = Mathf.Max(MinSlotColsRows.y, (int) (unmargined_max_pos.y / space_per_slot.y));
        
        Vector2 current_pos = start_pos;
        int index;
        for (int i = 0; i < CurrentColsRows.y; i++)
        {
            for (int j = 0; j < CurrentColsRows.x; j++)
            {
                index = i * CurrentColsRows.x + j;
                if (index >= TotalSlots)
                {
                    continue;
                }
                CreateSlot(current_pos, index);
                current_pos.x += Spacing.x + SlotDimensions.x;
            }

            current_pos.x = start_pos.x;
            current_pos.y -= Spacing.y + SlotDimensions.y;
        }
    }

    // -- UNITY


    private void Awake()
    {
        Initialize();
        CreateSlots();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMessengerTextsManager : UIVerticalTable
{
    // -- FIELDS
    
    
    private List<UIMessengerText> MessagesList = new List<UIMessengerText>();
    
    [SerializeField] private GameObject TextMessagePrefab;

    // -- METHODS


    public void AddText(TextMessage message)
    {
        var message_object = Instantiate(TextMessagePrefab,transform);
        var text_message = message_object.GetComponent<UIMessengerText>();

        text_message.Message = message;

        HorizontalAlignment alignment;
        alignment = message.Author.IsPlayer ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        
        AddItem(text_message,alignment);
    }

    
    
}

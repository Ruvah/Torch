using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMessengerTextsManager : UIObject
{
    // -- FIELDS
    
    
    private List<UIMessengerText> MessagesList = new List<UIMessengerText>();
    
    [SerializeField] private GameObject TextMessagePrefab;

    // -- METHODS


    public void AddText(TextMessage message)
    {
        var message_object = Instantiate(TextMessagePrefab,transform);
        var text_message = message_object.GetComponent<UIMessengerText>();
        //var previous_message = MessagesList.Last();
        //previous_message.transform
        
        MessagesList.Add(text_message);

        text_message.Message = message;
    }
    
}

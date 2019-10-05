using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessengerApp : UIContent
{
    // -- FIELDS


    private List<UIMessengerText> TextHistoryList;

    [SerializeField] private GameObject TextMessagesParent;
    [SerializeField] private GameObject TextMessagePrefab;
    
    // -- METHODS

    public void AddText(TextMessage text_message_message)
    {
        
    }
}

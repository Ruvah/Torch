using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessengerTester : MonoBehaviourSingleton<MessengerTester>
{
    // -- FIELDS

    public int AmountOfMessages;

    private MessengerApp MessengerApp;
    
    // -- METHODS

    private IEnumerator AddTexts()
    {
        for (int i = 0; i < AmountOfMessages; i++)
        {
            var message = new TextMessage();
            message.Text = "Hello";
            message.Author.IsPlayer = (i & 1) == 1;
            
            MessengerApp.TextMessagesParent.AddText(message);
            yield return new WaitForSeconds(1);
        }
    }
    
    // -- UNITY

    private void Start()
    {
        MessengerApp = HUDManager.Instance.MessengerApp;   
        StartCoroutine(AddTexts());
    }
}

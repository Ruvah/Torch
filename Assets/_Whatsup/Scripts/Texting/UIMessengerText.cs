using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMessengerText : UIObject
{
    // -- FIELDS

    public TextMessage Message
    {
        get => _Message;
        set
        {
            _Message = value;
            UpdateDisplay();
        }
    }

    private TextMessage _Message;
    [SerializeField] private TextMeshProUGUI TextDisplay;
    

    // -- METHODS


    public void UpdateDisplay()
    {
        TextDisplay.text = Message.Text;
    }

    // -- UNITY
}

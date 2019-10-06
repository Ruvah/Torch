using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // -- FIELDS


    private Outcome[] Outcomes;
    
    // -- METHODS

    public virtual void Interact()
    {
        foreach (var outcome in Outcomes)
        {
            outcome.SetActive();
        }
    }
    
    // -- UNITY

    protected void Awake()
    {
        Outcomes = GetComponents<Outcome>();
    }
}

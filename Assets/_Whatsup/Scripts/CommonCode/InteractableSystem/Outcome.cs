using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Outcome : MonoBehaviour
{
    // -- FIELDS

    [SerializeField] private bool DelayIsRealtime;
    [SerializeField] private float Delay;
    
    // -- METHODS

    public void SetActive()
    {
        if (Delay > 0)
        {
            StartCoroutine(ActivateAfterDelay());
        }
        else
        {
            OnActivate();
        }
    }

    protected abstract void OnActivate();

    private IEnumerator ActivateAfterDelay()
    {
        if (DelayIsRealtime)
        {
            yield return new WaitForSecondsRealtime(Delay);
        }
        else
        {
            yield return new WaitForSeconds(Delay);
        }
        
        OnActivate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHarvester : MonoBehaviour
{
    // -- TYPES


    // -- FIELDS

    public event CommonDelegates.VoidHandler OnChoppingStarted;
    public event CommonDelegates.VoidHandler OnHarvestingStopped;

    private ResourcePoint harvestTarget;

    public void HarvestTree(Tree tree)
    {
        if (tree == harvestTarget)
        {
            return;
        }

        harvestTarget = tree;
        OnChoppingStarted?.Invoke();
    }

    public void StopHarvesting()
    {
        harvestTarget = null;
        OnHarvestingStopped?.Invoke();
    }

    public void OnHarvestHit()
    {
        if (harvestTarget == null || harvestTarget.IsDepleted)
        {
            StopHarvesting();
            return;
        }
        harvestTarget.Hit();
    }
}

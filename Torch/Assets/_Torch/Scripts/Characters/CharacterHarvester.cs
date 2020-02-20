using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharacterHarvester : MonoBehaviour
{
    // -- TYPES

    // -- PROPERTIES

    public float HarvestDamage => harvestDamage;


    // -- FIELDS

    public event CommonDelegates.VoidHandler OnChoppingStarted;
    public event CommonDelegates.VoidHandler OnHarvestingStopped;

    public ControllableCharacter Character;

    private ResourcePoint harvestTarget;

    [SerializeField] private float harvestDamage;

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
        harvestTarget.Hit(this);
    }
}

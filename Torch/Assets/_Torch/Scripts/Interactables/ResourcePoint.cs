using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourcePoint : Interactable
{
    // -- PROPERTIES

    public bool IsDepleted => AmountOfResources == 0;

    // -- FIELDS

    public int AmountOfResources;
    public float Health;

    private float currentHealth;

    [SerializeField] private Item drop;


    // -- METHODS

    public void Hit(CharacterHarvester harvester)
    {
        if (IsDepleted)
        {
            return;
        }

        Debug.Log("ResourcePoint has been hit");

        currentHealth -= harvester.HarvestDamage;

        if (!(currentHealth <= 0))
        {
            return;
        }

        AmountOfResources--;
        currentHealth = Health;
        Debug.Log("character gets item from harvesting");
        harvester.Character.GiveItem(drop);
    }

    // -- UNITY

    private void Awake()
    {
        currentHealth = Health;
    }
}

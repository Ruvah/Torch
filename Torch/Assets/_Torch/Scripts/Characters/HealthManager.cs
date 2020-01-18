using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // -- PROPERTIES

    public float CurrentHealth
    {
        get;
        private set;
    }

    // -- PUBLIC

    [HideInInspector]
    public Character Character;

    // -- METHODS


    public void Damage(float amount)
    {

    }

    // -- UNITY

    public void Awake()
    {
        CurrentHealth = Character.CharacterStats.BaseHealth;
    }
}

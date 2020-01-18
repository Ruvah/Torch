using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [HideInInspector]
    public CharacterController Controller;
    public Ability BasicAttack;

    private List<Ability> Abilities = new List<Ability>();

    // -- METHODS

    private void UpdateAbilities()
    {
        BasicAttack.Update(Time.deltaTime);
        foreach (var ability in Abilities)
        {
            ability.Update(Time.deltaTime);
        }
    }

    // -- UNITY

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BasicAttack.Cast(Controller);
        }


    }
}

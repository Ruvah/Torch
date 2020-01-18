using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [HideInInspector]
    public Character Character;
    public Ability BasicAttack;

    private List<Ability> Abilities = new List<Ability>();

    // -- METHODS

    private void UpdateAbilities()
    {
        BasicAttack.UpdateAbility(Time.deltaTime);
        foreach (var ability in Abilities)
        {
            ability.UpdateAbility(Time.deltaTime);
        }
    }

    // -- UNITY

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BasicAttack.Cast(Character);
        }


    }
}

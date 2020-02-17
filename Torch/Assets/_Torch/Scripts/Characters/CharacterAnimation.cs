using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    // -- PROPERTIES

    // -- FIELDS

    private static int isChopping = Animator.StringToHash("IsChopping");

    [SerializeField] private Animator CharacterAnimator;
    [SerializeField] private ControllableCharacter character;

    // -- METHODS

    private void CharacterHarvester_OnChoppingStarted()
    {
        CharacterAnimator.SetBool(isChopping, true);
    }

    private void CharacterHarvester_OnHarvestingStopped()
    {
        CharacterAnimator.SetBool(isChopping, false);
    }

    // -- UNITY

    private void Awake()
    {
        character.Harvester.OnChoppingStarted += CharacterHarvester_OnChoppingStarted;
        character.Harvester.OnHarvestingStopped += CharacterHarvester_OnHarvestingStopped;
    }
}

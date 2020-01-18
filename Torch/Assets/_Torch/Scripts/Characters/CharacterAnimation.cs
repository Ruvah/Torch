using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    // -- PROPERTIES

    // -- FIELDS

    [HideInInspector]
    public CharacterController Character;


    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int DownwardHit = Animator.StringToHash("DownwardHack");


    [SerializeField] private Animator CharacterAnimator;

    // -- METHODS


    public void HitDownward()
    {
        CharacterAnimator.SetTrigger(DownwardHit);
    }

    // -- UNITY

    private void Update()
    {
        CharacterAnimator.SetFloat(MovementSpeed, Character.Movement.CurrentSpeed);
    }
}

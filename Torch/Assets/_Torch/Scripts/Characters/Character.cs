using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
   // -- PROPERTIES

   public CharacterStats CharacterStats => characterStats;
   public CharacterMovement Movement => movement;
   public CharacterCombat Combat => combat;
   public CharacterAnimation Animation => animation;

   // -- FIELDS

   [SerializeField] private CharacterStats characterStats;

   [SerializeField] private CharacterMovement movement;
   [SerializeField] private CharacterCombat combat;
   [SerializeField] private CharacterAnimation animation;
   [SerializeField] private CharacterHealth health;

   // -- UNITY

   private void Awake()
   {

      combat.Controller = this;
      animation.Character = this;
   }
}

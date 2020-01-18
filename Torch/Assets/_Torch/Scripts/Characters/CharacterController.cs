using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
   // -- FIELDS

   public CharacterMovement Movement;
   public CharacterCombat Combat;
   public CharacterAnimation Animation;


   // -- UNITY

   private void Awake()
   {
      Combat.Controller = this;
      Animation.Character = this;
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ITargeteable
{
   // -- PROPERTIES

   public CharacterStats CharacterStats => characterStats;
   public CharacterMovement Movement => movement;
   public CharacterCombat Combat => combat;
   public CharacterAnimation Animation => animation;
   public HealthManager Health => health;
   public Targeter Targeter => targeter;

   // -- FIELDS

   [SerializeField] private CharacterStats characterStats;

   [SerializeField] private CharacterMovement movement;
   [SerializeField] private CharacterCombat combat;
   [SerializeField] private CharacterAnimation animation;
   [SerializeField] private HealthManager health;
   [SerializeField] private Targeter targeter;

   // -- METHODS

   public virtual bool IsPlayerTargetable()
   {
      return true;
   }

   // -- UNITY

   private void Awake()
   {
      Health.Character = this;
      combat.Character = this;
      animation.Character = this;
      targeter.Character = this;
   }
}

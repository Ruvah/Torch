using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject2D))]
public class PlayerController : MonoBehaviour
{
   // -- FIELDS


   private float HorizontalMovement;

   [SerializeField] private float BaseMovementSpeed;
   [SerializeField] private PhysicsObject2D Motor2D;

   // -- UNITY

   private void Update()
   {
      HorizontalMovement = Input.GetAxis("Horizontal") * BaseMovementSpeed ;
   }

   private void FixedUpdate()
   {

   }
}

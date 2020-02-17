using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : PhysicsObject
{
    // -- PROPERTIES

    public float CurrentSpeed => TargetVelocity.magnitude;

    // -- FIELDS

    [SerializeField] protected float MovementSpeed;
    [SerializeField] protected float JumpForce = 7f;

    // -- METHODS


}

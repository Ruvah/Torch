using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : PhysicsObject
{
    // -- PROPERTIES

    public float CurrentSpeed => TargetVelocity.magnitude;

    // -- FIELDS

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce = 7f;

    // -- METHODS

    protected override void ComputeVelocity()
    {
        Vector3 move = Vector3.zero;

        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            _Velocity.y = JumpForce;
        }

        TargetVelocity = move * MovementSpeed;
    }
}

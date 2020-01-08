using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PhysicsObject2D
{
    // -- FIELDS

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce = 7f;

    // -- METHODS

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            _Velocity.y = JumpForce;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (_Velocity.y > 0)
            {
                _Velocity.y *= 0.5f;
            }
        }

        TargetVelocity = move * MovementSpeed;
    }
}

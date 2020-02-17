using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
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

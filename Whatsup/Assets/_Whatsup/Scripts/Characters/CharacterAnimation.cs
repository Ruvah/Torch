using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    // -- FIELDS

    private static readonly int HorizontalVelocity = Animator.StringToHash("HorizontalVelocity");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");


    [SerializeField] private float FlipXTreshold = 0.01f;
    [SerializeField] private Animator CharacterAnimator;
    [SerializeField] private PhysicsObject2D Motor;
    [SerializeField] private SpriteRenderer CharacterSprite;

    // -- METHODS

    private void UpdateMovementAnimation()
    {
        var velocityX = Motor.Velocity.x;
        var abs_hor_velocity = Mathf.Abs(velocityX);
        CharacterAnimator.SetFloat(HorizontalVelocity, abs_hor_velocity);
        CharacterAnimator.SetBool(IsGrounded, Motor.IsGrounded);
        if (abs_hor_velocity > FlipXTreshold)
        {
            CharacterSprite.flipX = velocityX > 0;
        }
    }

    // -- UNITY

    private void Update()
    {
        UpdateMovementAnimation();
    }
}

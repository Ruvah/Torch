using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// Tutorial followed : https://learn.unity.com/tutorial/live-session-2d-platformer-character-controller#5c7f8528edbc2a002053b68e
// Code might deviate from tutorial to personalize the physics


[RequireComponent(typeof(Rigidbody2D))]
public class PhysicsObject2D : MonoBehaviour
{
    // -- PROPERTIES


    public bool IsGrounded
    {
        get;
        protected set;
    }

    public Vector2 Velocity => _Velocity;


    // -- FIELDS

    protected const int BufferSize = 16;
    protected const float MinimumMoveDistance = 0.001f;

    protected Vector2 TargetVelocity;
    protected ContactFilter2D ContactFilter2D = new ContactFilter2D();
    protected RaycastHit2D[] HitBuffer;
    protected Vector2 GroundNormal;

    protected Vector2 _Velocity;


    [SerializeField] private float MinGroundNormalY = 0.65f;
    [SerializeField] private float ShellThickness = 0.01f;
    [SerializeField] private float GravityModifier;
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private CapsuleCollider2D Collider;


    public void ApplyMove(Vector2 delta, bool is_y_movement)
    {
        float distance = delta.magnitude;

        if (distance > MinimumMoveDistance)
        {
            int count = Rigidbody.Cast(delta, ContactFilter2D, HitBuffer, distance + ShellThickness);
            for (var i = 0; i < count; i++)
            {
                var hit = HitBuffer[i];
                Vector2 current_normal = hit.normal;
                if (current_normal.y > MinGroundNormalY)
                {
                    IsGrounded = true;
                    if (is_y_movement)
                    {
                        GroundNormal = current_normal;
                        current_normal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_Velocity, current_normal);

                if (projection < 0)
                {
                    _Velocity -= projection * current_normal;
                }

                float modified_distance = hit.distance - ShellThickness;
                distance = modified_distance < distance ? modified_distance : distance;
            }
        }

        Rigidbody.position += delta.normalized * distance;
    }

    protected virtual void ComputeVelocity()
    {

    }

    private void UpdateMovement()
    {
        _Velocity += GravityModifier * Time.deltaTime * Physics2D.gravity;
        _Velocity.x = TargetVelocity.x;

        IsGrounded = false;

        var delta_position = _Velocity * Time.deltaTime;
        //perpendicular vector from the ground normal
        Vector2 move_along_ground = new Vector2(GroundNormal.y, GroundNormal.x);

        //move X first as it goes works better with slopes
        Vector2 move = move_along_ground * delta_position.x;
        ApplyMove(move, false);

        move = Vector2.up * delta_position.y;

        ApplyMove(move, true);
    }

    // -- UNITY

    private void Update()
    {
        TargetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void Awake()
    {
        HitBuffer = new RaycastHit2D[BufferSize];
    }

    private void OnValidate()
    {
        if (Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.hideFlags = HideFlags.NotEditable;
            Rigidbody.isKinematic = true;
            Rigidbody.useFullKinematicContacts = true;
            Rigidbody.simulated = true;
        }

        ContactFilter2D.useTriggers = false;
        ContactFilter2D.layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
        ContactFilter2D.useLayerMask = true;
    }
}

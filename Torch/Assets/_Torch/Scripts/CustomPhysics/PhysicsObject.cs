using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public bool IsGrounded
    {
        get;
        protected set;
    }

    public Vector3 Velocity => _Velocity;


    // -- FIELDS

    protected const float MinimumMoveDistance = 0.001f;

    protected Vector3 TargetVelocity;
    protected Vector3 GroundNormal;

    protected Vector3 _Velocity;


    [SerializeField] private float MaxSlopeAngle;
    [SerializeField] private float MinGroundNormalY = 0.65f;
    [SerializeField] private float ShellThickness = 0.01f;
    [SerializeField] private float GravityModifier;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private CapsuleCollider Collider;


    public void ApplyMove(Vector3 delta, bool is_y_movement)
    {
        float distance = delta.magnitude;

        if (distance > MinimumMoveDistance)
        {
            var hits = Rigidbody.SweepTestAll(delta, distance + ShellThickness);
            foreach (var hit in hits)
            {
                Vector3 current_normal = hit.normal;
                if (current_normal.y > MinGroundNormalY)
                {
                    IsGrounded = true;
                    if (is_y_movement)
                    {
                        GroundNormal = current_normal;
                        current_normal.x = 0;
                    }
                }

                float projection = Vector3.Dot(_Velocity, current_normal);

                if (projection < MaxSlopeAngle)
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
        _Velocity += GravityModifier * Time.deltaTime * Physics.gravity;
        _Velocity.x = TargetVelocity.x;
        _Velocity.z = TargetVelocity.z;

        IsGrounded = false;

        var delta_position = _Velocity * Time.deltaTime;
        //perpendicular vector from the ground normal
        var move_along_ground_x = new Vector3(GroundNormal.y, GroundNormal.x, 0);
        var move_along_ground_z = new Vector3(0, GroundNormal.z, GroundNormal.y);


        //move X and Z first as it goes works better with slopes
        Vector3 move_x = move_along_ground_x * delta_position.x;
        Vector3 move_z = move_along_ground_z * delta_position.z;
        ApplyMove(move_x, false);
        ApplyMove(move_z, false);

        Vector3 move_y = Vector2.up * delta_position.y;

        ApplyMove(move_y, true);
    }

    // -- UNITY

    private void Update()
    {
        TargetVelocity = Vector3.zero;
        ComputeVelocity();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }


    private void OnValidate()
    {
        if (Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
        if (!Rigidbody)
        {
            Rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        else
        {
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.hideFlags = HideFlags.NotEditable;
            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;
        }
    }
}

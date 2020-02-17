using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMotor : MonoBehaviour
{
    // -- FIELDS

    private Transform followTarget;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float angularSpeed;

    // -- METHODS

    public void MoveTo(Vector3 position)
    {
        followTarget = null;
        navMeshAgent.updateRotation = true;
        navMeshAgent.destination = position;
    }

    public void FollowTarget(Transform to_follow)
    {
        followTarget = to_follow;
        navMeshAgent.updateRotation = false;
        navMeshAgent.destination = followTarget.position;
    }

    public void StopFollowing()
    {
        followTarget = null;
        navMeshAgent.destination = transform.position;
    }

    private void FaceTarget()
    {
        Vector3 direction = followTarget.position - transform.position;
        direction.Normalize();
        direction.y = 0;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion look_rotation = Quaternion.LookRotation(direction);
        var rotation = transform.rotation;
        float angle = Quaternion.Angle(rotation, look_rotation);
        float time_to_complete = (angle / angularSpeed);
        float interpolation = Mathf.Min(1, Time.fixedDeltaTime / time_to_complete);

        rotation = Quaternion.Slerp(rotation, look_rotation, interpolation);
        transform.rotation = rotation;
    }

    // -- UNITY

    private void FixedUpdate()
    {
        if (followTarget == null)
        {
            return;
        }

        navMeshAgent.destination = followTarget.position;
        FaceTarget();
    }
}

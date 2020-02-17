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

    // -- METHODS

    public void MoveTo(Vector3 position)
    {
        followTarget = null;
        navMeshAgent.destination = position;
    }

    public void FollowTarget(Transform to_follow)
    {
        followTarget = to_follow;
        navMeshAgent.destination = followTarget.position;
    }

    // -- UNITY

    private void Update()
    {
        if (followTarget)
        {
            navMeshAgent.destination = followTarget.position;
        }
    }
}

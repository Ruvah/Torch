using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    // -- PROPERTIES

    public Interactable Target
    {
        get => target;
    }

    public CharacterHarvester Harvester => harvester;

    // -- FIELDS

    private Interactable target;

    [SerializeField] private CharacterHarvester harvester;
    [SerializeField] private CharacterMotor motor;

    // -- METHODS

    public void SetTarget(Interactable interactable)
    {
        target = interactable;
        motor.FollowTarget(target.transform);
    }

    public void ClearTarget()
    {
        harvester.StopHarvesting();
        motor.StopFollowing();
        target = null;
    }

    public void MoveTo(Vector3 position)
    {
        harvester.StopHarvesting();
        motor.MoveTo(position);
    }

    // -- UNITY

    private void Update()
    {
        if (Target != null && Vector3.Distance(Target.transform.position, transform.position) <= Target.InteractionRadius)
        {
            Target.Interact(this);
        }
    }
}

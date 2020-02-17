using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableCharacter : MonoBehaviour
{
    // -- PROPERTIES

    public Interactable Target
    {
        get => target;
        set
        {
            target = value;
            motor.FollowTarget(target.transform);
        }
    }

    // -- FIELDS

    private Interactable target;

    [SerializeField] private CharacterMotor motor;

    // -- METHODS

    public void MoveTo(Vector3 position)
    {
        motor.MoveTo(position);
    }

    public void SetTarget(Interactable target_to_be)
    {
        target = target_to_be;
    }
}

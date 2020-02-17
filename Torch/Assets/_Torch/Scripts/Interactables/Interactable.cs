using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // -- PROPERTIES

    public float InteractionRadius => interactionRadius;

    // -- FIELDS


    [SerializeField] private float interactionRadius;


    // -- METHODS


    public abstract void Interact(ControllableCharacter character);
}

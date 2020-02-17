using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // -- PROPERTIES


    // -- FIELDS

    private ControllableCharacter selectedCharacter;
    private RaycastHit[] raycastBuffer = new RaycastHit[16];
    private Camera camera;

    // -- METHODS

    private int RayFromMouseNonAlloc()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        return Physics.RaycastNonAlloc(ray, raycastBuffer, 100);
    }

    private bool RayFromMouse(out RaycastHit hit, float distance)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, distance);
    }

    private void SelectSingleUnit()
    {
        if (!RayFromMouse( out var hit, 100))
        {
            return;
        }

        var other = hit.collider;
        if (!other.CompareTag("Unit"))
        {
            return;
        }

        var character = other.GetComponent<ControllableCharacter>();
        if (character != null)
        {
            selectedCharacter = character;
        }
    }

    private void IssueCommand()
    {
        if (selectedCharacter == null || !RayFromMouse( out var hit, 100))
        {
            return;
        }

        var other = hit.collider;
        var interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            selectedCharacter.SetTarget(interactable);
            return;
        }

        selectedCharacter.ClearTarget();
        selectedCharacter.MoveTo(hit.point);
    }

    // -- UNITY

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectSingleUnit();
        }

        if (Input.GetMouseButtonDown(1))
        {
            IssueCommand();
        }
    }
}

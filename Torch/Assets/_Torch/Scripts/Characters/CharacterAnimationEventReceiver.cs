using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEventReceiver : MonoBehaviour
{
    // -- FIELDS


    [SerializeField] private ControllableCharacter character;

    // -- METHODS

    public void HarvestHit()
    {
        character.Harvester.OnHarvestHit();
    }
}

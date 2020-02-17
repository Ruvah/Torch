using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : ResourcePoint
{
    public override void Interact(ControllableCharacter character)
    {
        character.Harvester.HarvestTree(this);
    }
}

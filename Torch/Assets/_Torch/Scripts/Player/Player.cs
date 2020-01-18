using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // -- PROPERTIES


    // -- FIELDS


    // -- METHODS

    public override bool IsPlayerTargetable()
    {
        return false;
    }
}

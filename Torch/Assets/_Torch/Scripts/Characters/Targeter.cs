using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    // -- PROPERTIES

    public ITargeteable Target
    {
        get { return target; }
        private set { target = value; }
    }



    // -- FIELDS

    [HideInInspector]
    public Character Character;

    private ITargeteable target;

    // -- METHODS

    public void SetTarget(ITargeteable new_target)
    {
        Target = new_target;
    }

}

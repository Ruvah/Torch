using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    // -- PROPERTIES

    public bool IsActive
    {
        get;
        protected set;
    }

    // -- FIELDS

    public float CurrentCooldown;
    protected CharacterController Caster;

    // -- METHODS

    public abstract void Cast(CharacterController caster);

    public virtual void Update(float delta_time)
    {
        CurrentCooldown -= delta_time;
        CurrentCooldown = Mathf.Max(CurrentCooldown, 0);
    }
}

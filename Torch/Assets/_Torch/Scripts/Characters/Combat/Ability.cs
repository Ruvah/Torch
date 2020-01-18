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
    protected Character Caster;
    protected ITargeteable Target;

    // -- METHODS

    public virtual void Cast(Character caster)
    {
        Caster = caster;
        Target = Caster.Targeter.Target;
    }

    public virtual void UpdateAbility(float delta_time)
    {
        CurrentCooldown -= delta_time;
        CurrentCooldown = Mathf.Max(CurrentCooldown, 0);
    }
}

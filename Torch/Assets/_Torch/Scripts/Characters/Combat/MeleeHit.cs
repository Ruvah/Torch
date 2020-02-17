using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeHit", menuName =  "Abilities/MeleeHit")]
public class MeleeHit : DurationAbility
{
    public float DamageRadius;
    public LayerMask ToHit;

    // -- METHODS


    public override void Cast(Character caster)
    {
        base.Cast(caster);
        caster.Animation.HitDownward();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeter : Targeter
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var results = Physics.OverlapSphere(Character.Movement.transform.position, 20);
            foreach (var result in results)
            {
                var target = result.GetComponent<ITargeteable>();
                if (target != null && !ReferenceEquals(target, Character))
                {
                    SetTarget(target);
                }
            }
        }
    }
}

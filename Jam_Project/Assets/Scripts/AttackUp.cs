using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : Pickup
{
    private void Start()
    {
        info = "+20% Attack Damage";
    }

    public override void ApplyEffect(GameObject go)
    {
        go.transform.Find("Weapon").GetComponent<Weapon>().attackBonus += 20.0f;
    }
}

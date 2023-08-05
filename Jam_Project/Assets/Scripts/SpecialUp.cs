using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUp : Pickup
{
    private void Start()
    {
        info = "+20% Special Damage";
    }
    public override void ApplyEffect(GameObject go) 
    {
        go.transform.Find("Weapon").GetComponent<Weapon>().specialBonus += 20.0f;
    }
}

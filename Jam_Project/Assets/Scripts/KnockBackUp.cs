using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackUp : Pickup
{
    private void Start()
    {
        info = "Knock back +";
    }
    public override void ApplyEffect(GameObject go)
    {
        go.transform.Find("Weapon").GetComponent<Weapon>().knockBack += 3.0f;
    }
}

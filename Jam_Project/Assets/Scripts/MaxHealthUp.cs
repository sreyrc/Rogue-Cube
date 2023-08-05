using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthUp : Pickup
{
    private void Start()
    {
        info = "+50 Max Health";
    }
    public override void ApplyEffect(GameObject go)
    {
        go.GetComponent<CharacterStats>().maxHp += 50.0f;
    }
}

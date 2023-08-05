using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishAllHealth : Pickup
{
    private void Start()
    {
        info = "Replenish all health";
    }
    public override void ApplyEffect(GameObject go)
    {
        CharacterStats stats = go.GetComponent<CharacterStats>();
        stats.Hp = stats.maxHp;
    }
}

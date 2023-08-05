using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : Pickup
{
    private void Start()
    {
        info = "Heal";
    }
    public override void ApplyEffect(GameObject go)
    {
        go.GetComponent<CharacterStats>().Hp += 20.0f;
    }
}

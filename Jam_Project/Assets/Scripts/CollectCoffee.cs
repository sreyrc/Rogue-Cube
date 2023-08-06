using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoffee : Pickup
{
    [SerializeField] WinScreenUIActivate winScreenUIActivator;
    [SerializeField] GameObject coffeeCollectEffect;

    [SerializeField] GameObject coffeeEffect;
    public override void ApplyEffect(GameObject obj)
    {
        coffeeEffect.SetActive(false);
        Instantiate(coffeeCollectEffect, transform.position, Quaternion.identity);
        winScreenUIActivator.ActivateWinScreen();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractionLogic : MonoBehaviour
{
    CharacterStats playerStats;
    GameObject pickupWithinRange;

    [SerializeField] TextMeshPro infoDisplay; 
    private void Start()
    {
        playerStats = GetComponent<CharacterStats>();
    }

    private void OnTriggerStay(Collider other)
    {
        Damager damager = other.gameObject.GetComponent<Damager>();
        if (damager != null)
        {
            damager.InflictDamage(playerStats);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickup pickup = other.gameObject.GetComponent<Pickup>();
        if (pickup != null)
        {
            infoDisplay.text = pickup.Info;
            infoDisplay.transform.position = other.transform.position + Vector3.up * 2.0f;
            pickup.ApplyEffect(transform.gameObject);
            pickupWithinRange = other.gameObject;
            //Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Pickup>() != null)
        {
            infoDisplay.text = "";
            pickupWithinRange = null;
        }
    }

    private void Update()
    {
        if (pickupWithinRange != null && Input.GetKeyDown(KeyCode.E))
        {
            pickupWithinRange.GetComponent<Pickup>().ApplyEffect(gameObject);
            Destroy(pickupWithinRange);
            pickupWithinRange = null;
            infoDisplay.text = "";
        }
    }
}

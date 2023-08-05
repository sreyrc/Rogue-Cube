using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractionLogic : MonoBehaviour
{
    CharacterStats playerStats;
    PlayerDash playerDash;
    GameObject pickupWithinRange;
    public bool isHurt = false;

    [SerializeField] TextMeshPro infoDisplay; 
    private void Start()
    {
        playerStats = GetComponent<CharacterStats>();
        playerDash = GetComponent<PlayerDash>();
    }

    private void OnTriggerStay(Collider other)
    {
        Damager damager = other.gameObject.GetComponent<Damager>();
        if (damager != null && !playerDash.IsDashing)
        {
            damager.InflictDamage(playerStats);
            isHurt = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickup pickup = other.gameObject.GetComponent<Pickup>();
        if (pickup != null)
        {
            infoDisplay.text = pickup.Info;
            infoDisplay.transform.position = other.transform.position + Vector3.up * 2.0f;
            pickupWithinRange = other.gameObject;
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

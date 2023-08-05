using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GravityActivator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var navMeshAgent = other.gameObject.GetComponent<NavMeshAgent>();
        var enemyAIMove = other.gameObject.GetComponent<EnemyAIMoveTo>();

        if (navMeshAgent != null) { Destroy(navMeshAgent); }
        if (enemyAIMove != null) { Destroy(enemyAIMove); }

        var playerDash = other.gameObject.GetComponent<PlayerDash>();

        if (playerDash != null) 
        { 
            if (playerDash.IsDashing) { return; } 
        }

        other.gameObject.AddComponent<Fall>();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    
    EnemyAIMoveTo enemyAIMoveTo;
    Animator animator;
    BoxCollider boxCollider;

    [SerializeField] float attackRadius;
    [SerializeField] float fov;
    bool playerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAIMoveTo = GetComponent<EnemyAIMoveTo>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.DrawLine(transform.position, player.transform.position, Color.white);

        // Player must be within enemy's attack radius and
        // Player must be in enemie's FoV
        UnityEngine.Vector3 vecToPlayer = player.transform.position - transform.position;
        vecToPlayer.Normalize();
        Debug.DrawRay(transform.position, vecToPlayer, Color.blue);

        if (UnityEngine.Vector3.Distance(transform.position, player.transform.position) < attackRadius
            && (Mathf.Rad2Deg * Mathf.Acos(UnityEngine.Vector3.Dot(vecToPlayer, transform.forward))) < fov)  
        {
            boxCollider.enabled = true;
            animator.SetBool("playerInRange", true);
            playerInRange = true;
        }
        else if (playerInRange)
        {
            boxCollider.enabled = false;
            animator.SetBool("playerInRange", false);
            playerInRange = false;
        }
    }
}

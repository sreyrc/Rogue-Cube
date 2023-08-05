using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    
    EnemyAIMoveTo enemyAIMoveTo;
    Animator animator;
    Damager[] damagers;

    [SerializeField] float attackRadius;
    [SerializeField] float fov;
    [SerializeField] bool playerInRange = false;
    bool enableCollider = false;
    [SerializeField] float timeTillEnableCollider = Mathf.Infinity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAIMoveTo = GetComponent<EnemyAIMoveTo>();
        animator = GetComponent<Animator>();
        damagers = GetComponentsInChildren<Damager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Player must be within enemy's attack radius and
        // Player must be in enemie's FoV
        UnityEngine.Vector3 vecToPlayer = player.transform.position - transform.position;
        vecToPlayer.Normalize();
        Debug.DrawRay(transform.position, vecToPlayer, Color.blue);

        if (UnityEngine.Vector3.Distance(transform.position, player.transform.position) < attackRadius
            && (Mathf.Rad2Deg * Mathf.Acos(UnityEngine.Vector3.Dot(vecToPlayer, transform.forward))) < fov)  
        {
            if (!playerInRange) {
                playerInRange = true;
                animator.SetBool("playerInRange", true);
                timeTillEnableCollider = 2.0f;
            }
            else { timeTillEnableCollider -= Time.deltaTime; }

            if (timeTillEnableCollider <= 0.0f && !enableCollider) {
                timeTillEnableCollider = 0.0f;
                damagers[0].enabled = true;
                damagers[1].enabled = true;
            }          

        }
        else if (playerInRange)
        {
            enableCollider = false;
            damagers[0].enabled = false;
            damagers[1].enabled = false;
            playerInRange = false;
            timeTillEnableCollider = Mathf.Infinity;
            animator.SetBool("playerInRange", false);
        }
    }
}

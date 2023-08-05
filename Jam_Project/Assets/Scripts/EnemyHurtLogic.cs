using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyHurtLogic : MonoBehaviour
{
    [SerializeField] private float stunTimer = 0.0f;
    [SerializeField] private float stunDuration = 1.0f;

    [SerializeField] private float hitCoolDownDuration = 0.03f;
    [SerializeField] private float hitCoolDownTime = 0.0f;

    [SerializeField] float attackRadius = 3.0f;

    GameObject player;
    Weapon playerWeapon;
    PlayerAttack playerAttackComp;
    
    public GameObject healthBar;

    public bool emitParticles = false;
    public bool deathParticlesEmitted = false;
    public bool isStunned = false;
    
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private EnemyAIMoveTo enemyAIMoveTo;
    private CharacterStats stats;

    Vector3 pushBackDirection = Vector3.zero;

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAIMoveTo = GetComponent<EnemyAIMoveTo>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerWeapon = player.GetComponentInChildren<Weapon>();
        playerAttackComp = player.GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        // The following conditions need to be satisfied for the enemy to get hurt
        // 1. Player is within range and is in player's fov (180)
        // 2. Enemy's hit cooldown is zero or back to zero after a hit - Ready to get hurt again
        // 3. Player's weapon is being used.
        // Don't want the weapon to hurt the enemy when the enemy collides with the weapon
        // but the player is not attacking

        // Refactor if time permits. Whether player can see enemy should be player's logic - not enemy's
        Vector3 playerToEnemy = transform.position - player.transform.position;
        float playerToEnemyDistance = playerToEnemy.magnitude;
        playerToEnemy.Normalize();
        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(playerToEnemy, player.transform.forward));

        bool attackStatus = playerAttackComp.GetAttackStatus(AttackType.Normal);
        bool specialStatus = playerAttackComp.GetAttackStatus(AttackType.Special);

        if (playerToEnemyDistance < attackRadius && 
            ((attackStatus && angle < 45.0f) || specialStatus)
            && hitCoolDownTime <= 0.0f 
            && playerWeapon.inUse)
        {
            // Hp reduced of this GO = attack damage of weapon attacking this GO

            if (attackStatus) { stats.Hp -= playerWeapon.AttackDamage; }
            else { stats.Hp -= playerWeapon.SpecialDamage; }

            hitCoolDownTime = hitCoolDownDuration;

            // Animation transition
            if (animator != null) { animator.SetBool("isHit", true); }

            // Give an indicator that particles should be emitted
            emitParticles = true;

            // Stun mechanic is only for animation
            stunTimer = stunDuration;
            isStunned = true;

            pushBackDirection = new Vector3(playerToEnemy.x,
                0.0f, playerToEnemy.z);

            pushBackDirection.Normalize();

            if (navMeshAgent != null) { navMeshAgent.enabled = false; }
            if (enemyAIMoveTo != null) { enemyAIMoveTo.enabled = false; }
        }

        Debug.DrawRay(transform.position, pushBackDirection, Color.black);

        if (hitCoolDownTime > 0.0f)
        {
            hitCoolDownTime -= Time.deltaTime;
        }

        if (stunTimer > 0.0f)
        {
            stunTimer -= Time.deltaTime;

            // Push speed decreases with time
            // Desired effect: Enemy gets a knock back and decelerates to a full stop before being unstunned
            if (pushBackDirection != Vector3.zero)
            {
                float k = Mathf.Lerp(0, playerWeapon.knockBack, stunTimer/stunDuration);
                transform.Translate(pushBackDirection * k * Time.deltaTime, Space.World);
            }
        }    
        else if (isStunned)
        {

            if (navMeshAgent != null) { navMeshAgent.enabled = true; }
            if (enemyAIMoveTo != null) { enemyAIMoveTo.enabled = true; }

            if (animator != null)
            {
                animator.SetBool("isHit", false);
            }
            isStunned = false;
        }
    }
}

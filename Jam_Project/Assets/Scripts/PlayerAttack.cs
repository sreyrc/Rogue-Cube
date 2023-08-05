using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackType
{
    Normal,
    Special
}

public class PlayerAttack : MonoBehaviour
{
    // Two trackers each - 
    // Index 0 for Normal Attack
    // Index 1 for Special Attack

    [SerializeField] private float[] attackCoolDownDuration = new float[2];
    [SerializeField] private bool[] isAttacking = new bool[2];
    [SerializeField] private float[] attackCoolDownTimer = new float[2];

    GameObject slash;

    public bool GetAttackStatus(AttackType type)
    {
        return isAttacking[(int)type];
    }
    public bool CanAttack()
    {
        return attackCoolDownTimer[(int)AttackType.Normal] <= 0.0f
                && attackCoolDownTimer[(int)AttackType.Special] <= 0.0f;
    }

    private void Start()
    {
        attackCoolDownTimer[(int)AttackType.Normal] = 0.0f;   
        attackCoolDownTimer[(int)AttackType.Special] = 0.0f;

        slash = transform.Find("Weapon").GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            // Make sure both attack and special are free for use to 
            // use either attack or special
            if (Input.GetMouseButtonDown(i) && CanAttack())
            {
                isAttacking[i] = true;
                GetComponentInChildren<Weapon>().inUse = true;
                attackCoolDownTimer[i] = attackCoolDownDuration[i];
                slash.SetActive(true);
            }

            if (attackCoolDownTimer[i] > 0.0f)
            {
                attackCoolDownTimer[i] -= Time.deltaTime;
            }
            // Make sure both timers are below zero to enable any attacks
            else if (CanAttack())
            {
                isAttacking[i] = false;
                slash.SetActive(false);
                isAttacking[(i + 1) % 2] = false;
                GetComponentInChildren<Weapon>().inUse = false;
            }
        }

    }
}

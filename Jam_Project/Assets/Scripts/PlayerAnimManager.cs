using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    Animator[] animators;
    PlayerAttack attacker;
    bool isIdle = true;

    // Start is called before the first frame update
    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        attacker = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attacker.GetAttackStatus(AttackType.Normal) && isIdle)
        {
            foreach(var anim in animators)
            {
                anim.SetBool("isAttacking", true);
            }
            isIdle = false;
        }
        else if(attacker.GetAttackStatus(AttackType.Special) && isIdle)
        {
            foreach (var anim in animators)
            {
                anim.SetBool("isDoingSpecial", true);
            }
            isIdle = false;
        }
        else {
            if (!isIdle && attacker.CanAttack())
            {
                foreach (var anim in animators)
                {
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isDoingSpecial", false);
                }
                isIdle = true;
            }
        }
    }
}

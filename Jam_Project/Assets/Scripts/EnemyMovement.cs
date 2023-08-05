using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameObject player;
    CharacterStats characterStats;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = player.transform.position - transform.position;
        moveDirection.Normalize();

        transform.Translate(moveDirection * characterStats.moveSpeed * Time.deltaTime, Space.World);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation,
                characterStats.rotationSpeed * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] float pauseBetweenHurt = 1.0f;
    [SerializeField] public float damage = 1.0f;

    float timer = 0.0f;

    public void InflictDamage(CharacterStats stats)
    {
        if (timer <= 0.0f)
        {
            stats.Hp -= damage;
            timer = pauseBetweenHurt;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }
}

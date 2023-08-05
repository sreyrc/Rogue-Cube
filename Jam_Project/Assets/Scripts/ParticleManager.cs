using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hitParticles;
    EnemyManager enemyManager;

    // Update is called once per frame
    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    void Update()
    {
        // This looping is inefficient. Change if you can
        // Particles for enemy hits
        for(int i = 0; i < enemyManager.Enemies.Count; i++) 
        {
            var enemyHitComp = enemyManager.Enemies[i].GetComponent<EnemyHurtLogic>();
            if (enemyHitComp.emitParticles)
            {
                var hitParticlesIntance = Instantiate(hitParticles);
                hitParticlesIntance.transform.position =
                    enemyManager.Enemies[i].transform.position;
                enemyHitComp.emitParticles = false;
            }

            if (enemyManager.Enemies[i].GetComponent<CharacterStats>().Hp <= 0)
            {
                enemyHitComp.deathParticlesEmitted = true;
            }
        }    
    }
}

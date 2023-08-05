using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    // Have an array of enemies that can be spawned
    // Set this up when you have more than one enemy type
    public GameObject[] enemyPrefabs;

    [SerializeField] GameObject healthBar;

    private List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> Enemies
    {
        get { return enemies; }
    }

    [SerializeField] Vector2Int enemyIndexBounds;
    [SerializeField] Vector2Int enemyCountMinMax;

    private void Update()
    {
        // TODO: Not the biggest deal right now - but definitely inefficient
        // Use a dictionary or something
        for (int i = 0; i < enemies.Count; i++)
        {
            // If Particles manager emitted the death particles for this,
            // then only destroy this object. The order matters
            if (enemies[i].GetComponent<EnemyHurtLogic>().deathParticlesEmitted)
            {
                var enemy = enemies[i];
                enemies.Remove(enemies[i]);
                Destroy(enemy);
            }
        }
    }

    // Update is called once per frame
    public void SpawnEnemies()
    {
        int enemyCount = Random.Range(enemyCountMinMax.x, enemyCountMinMax.y);
        for(int i = 0; i < enemyCount; i++)
        {
            // Modify spawning logic later down the line
            Vector3 spawnPosition =
                new Vector3(Random.Range(-10, 10), 1.3f, Random.Range(-10, 10));

            int enemyType = Random.Range(enemyIndexBounds.x, enemyIndexBounds.y);
                
            GameObject enemy = Instantiate(enemyPrefabs[enemyType], spawnPosition, Quaternion.identity);

            var healthbar = Instantiate(healthBar, spawnPosition + Vector3.up, Quaternion.identity);
            healthbar.transform.parent = enemy.transform;

            enemies.Add(enemy);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    // Have an array of enemies that can be spawned
    // Set this up when you have more than one enemy type
    public GameObject[] enemyPrefabs;

    [SerializeField] GameObject healthBar;

    //private List<GameObject> enemies = new List<GameObject>();
    private Dictionary<string, GameObject> enemyMap = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> EnemyMap
    {
        get { return enemyMap; }
    }

    [SerializeField] Vector2Int enemyIndexBounds;
    [SerializeField] Vector2Int enemyCountMinMax;

    // Cursed encounter
    // Enemies deal twice as much damage,
    // have twice as much health and
    // have move twice as fast
    public void ApplyCurse()
    {
        foreach (KeyValuePair<string, GameObject> enemy in enemyMap)
        {
            CharacterStats enemyStats = enemy.Value.GetComponent<CharacterStats>();
            enemyStats.maxHp *= 3.0f;
            enemyStats.Hp *= 3.0f;

            Damager damager = enemy.Value.GetComponentInChildren<Damager>();
            if (damager != null) { damager.damage *= 2.0f; }

            NavMeshAgent agent = enemy.Value.GetComponent<NavMeshAgent>();
            agent.speed *= 2.0f;

            var renderer = enemy.Value.GetComponentInChildren<MeshRenderer>();
            renderer.material.color = new Color(0, 0, 0);
        }
    }

    public void KillEnemy(string enemyName)
    {
        GameObject enemy = enemyMap[enemyName];
        enemyMap.Remove(enemyName);
        Destroy(enemy);
    }

    public void ActivateColliders()
    {
        foreach (KeyValuePair<string, GameObject> enemy in enemyMap)
        {
            enemy.Value.GetComponent<Collider>().enabled = true;
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

            enemy.name = "Enemy" + i.ToString();

            enemyMap.Add(enemy.name, enemy);
        }
        Invoke("ActivateColliders", 2.0f);
    }
}

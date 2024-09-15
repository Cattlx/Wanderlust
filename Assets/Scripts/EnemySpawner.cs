using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public int EnemiesPerSpawnpoint = 0;
    public List<Transform> SpawnPoints = new List<Transform>();

    private void Awake()
    {
        for (int i = 0; i < EnemiesPerSpawnpoint; i++)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in SpawnPoints)
        {
            Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}

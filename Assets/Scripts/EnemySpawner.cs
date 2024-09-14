using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public List<Transform> SpawnPoints = new List<Transform>();

    private void Awake()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in SpawnPoints)
        {
            Instantiate(EnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}

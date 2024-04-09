using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private List<EnemyPath> paths;
    [SerializeField]
    private float spawningDelay;
    [SerializeField]
    private Enemy enemyPrefab;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawningDelay);
            Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.Path = paths[Random.Range(0, paths.Count)];
        }
    }
}

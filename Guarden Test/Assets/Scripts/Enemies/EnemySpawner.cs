using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<EnemyPath> paths;
    [SerializeField]
    private Enemy enemyPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Enemy enemy = Instantiate(enemyPrefab);
            enemy.Path = paths[0];
            Debug.Log("Enemy Spawned");
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontenSpawn : MonoBehaviour
{
    List<GameObject> allEnemies = new List<GameObject>();
    float randomUnitCircleRadius;
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
    [SerializeField] GameObject enemy;
    [SerializeField] int amountofEnemies;

    [Header("Scritable Object")]
    [SerializeField] BaseStats[] baseStats;
    RaycastHit hit;
    [SerializeField] LayerMask mask;
    Vector3 newWorldpos;
    void Start()
    {
        SpawnEnemies();
    }
    void SpawnEnemies()
    {
        for (int i = 0; i < amountofEnemies; i++)
        {
            var index = Random.Range(0, baseStats.Length);
            //pick a random position amount
            randomUnitCircleRadius = Random.Range(minRadius, maxRadius);
            // pick a random spawn point in the radius
            Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, 0, Random.insideUnitCircle.y * randomUnitCircleRadius);
            // spawn a enemy
            Transform newEnemy = Instantiate(enemy, transform.position, transform.rotation).transform;
            //set the random position to the target
            newEnemy.GetComponent<EnemyBehaviour>().target = newPos;
            // give the newenemy his components and stats
            newEnemy.GetComponent<EnemyBehaviour>().baseStats = baseStats[index];
            GetComponent<CheckState>().Checkstate(newEnemy.GetComponent<EnemyBehaviour>().baseStats, newEnemy.gameObject);
            // set the enemy spawnposition
            newPos = new Vector3(0, 0, 0);
        }
    }
}

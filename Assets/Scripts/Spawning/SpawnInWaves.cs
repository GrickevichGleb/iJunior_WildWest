using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInWaves : MonoBehaviour
{
    [SerializeField] private int _numberOfWaves = 3;
    [SerializeField] private int _numberOfEnemiesInWave = 4;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _intervalBetweenWaves = 4f;
    
    [SerializeField] private EnemySpawner _enemySpawner;

    private int _currentWave = 1;
    private int _spawnedEnemiesInThisWave = 0;
    
    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (_currentWave <= _numberOfWaves)
        {
            while (_spawnedEnemiesInThisWave <= _numberOfEnemiesInWave)
            {
                yield return new WaitForSeconds(_spawnInterval);
            
                _enemySpawner.SpawnEnemy();
                _spawnedEnemiesInThisWave++;
            }

            yield return new WaitForSeconds(_intervalBetweenWaves);

            _currentWave++;
            _spawnedEnemiesInThisWave = 0;
        }
    }
}

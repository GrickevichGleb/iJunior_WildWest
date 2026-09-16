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

    private Coroutine _spawningCoroutine;
    
    private void Start()
    {
        _spawningCoroutine = StartCoroutine(SpawnCoroutine());
    }

    public void ResetWaves()
    {
        _currentWave = 1;
        _spawnedEnemiesInThisWave = 0;
        
        if(_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);

        _spawningCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (_currentWave <= _numberOfWaves)
        {
            Debug.Log("Wave started: " + _currentWave);
            
            while (_spawnedEnemiesInThisWave < _numberOfEnemiesInWave)
            {
                yield return new WaitForSeconds(_spawnInterval);
            
                _enemySpawner.SpawnEnemy();
                _spawnedEnemiesInThisWave++;
            }
            Debug.Log($"Wave {_currentWave} finished");
            yield return new WaitForSeconds(_intervalBetweenWaves);

            _currentWave++;
            _spawnedEnemiesInThisWave = 0;
        }
    }
}

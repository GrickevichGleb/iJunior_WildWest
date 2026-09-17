using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInWaves : StatValue
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
        UpdateStat();
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

    public bool IsAllEnemiesSpawned()
    {
        if (_currentWave == _numberOfWaves && _spawnedEnemiesInThisWave == _numberOfEnemiesInWave)
            return true;

        return false;
    }

    private IEnumerator SpawnCoroutine()
    {
        while (_currentWave <= _numberOfWaves)
        {
            _spawnedEnemiesInThisWave = 0;
            
            while (_spawnedEnemiesInThisWave < _numberOfEnemiesInWave)
            {
                yield return new WaitForSeconds(_spawnInterval);
                
                _enemySpawner.SpawnEnemy();
                _spawnedEnemiesInThisWave++;
            }

            yield return new WaitUntil(() => _enemySpawner.GetActiveEnemiesCount() == 0);
            
            yield return new WaitForSeconds(_intervalBetweenWaves);

            _currentWave++;
            
            UpdateStat();
        }
    }

    private void UpdateStat()
    {
        Current = _currentWave;
        Max = _numberOfWaves;
        
        InvokeChanged();
    }
}

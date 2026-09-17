using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner<CharEnemy>
{
    [SerializeField] private Transform _attackTarget;

    [SerializeField] private SpawnPoints _spawnPoints;

    private Transform _spawnPoint;

    public event Action EnemyDead;
    
    public void SpawnEnemy()
    {
        if(_spawnPoints.TryGetPoint(out _spawnPoint))
            Pool.Get();
    }

    public int GetActiveEnemiesCount()
    {
        return ActiveObjects.Count;
    }

    protected override void ActionOnGet(CharEnemy spawnable)
    {
        base.ActionOnGet(spawnable);

        spawnable.Initialize(_spawnPoint.position, _attackTarget);
    }

    protected override void OnRequestRelease(Spawnable spawnable)
    {
        base.OnRequestRelease(spawnable);

        EnemyDead?.Invoke();
    }
}

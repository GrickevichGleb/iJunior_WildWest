using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner<CharEnemy>
{
    [SerializeField] private Transform _attackTarget;

    [SerializeField] private SpawnPoints _spawnPoints;

    private Transform _spawnPoint;
    
    public void SpawnEnemy()
    {
        if(_spawnPoints.TryGetPoint(out _spawnPoint))
            Pool.Get();
    }

    protected override void ActionOnGet(CharEnemy spawnable)
    {
        base.ActionOnGet(spawnable);

        spawnable.Initialize(_spawnPoint.position, _attackTarget);
    }
}

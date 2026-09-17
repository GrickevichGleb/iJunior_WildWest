using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private const string VictoryTitle = "VICTORY";
    private const string DefeatTitle = "DEFEAT";
    
    [SerializeField] private CharPlayer _player;
    [SerializeField] private Transform _playerStartPosition;
    [Space] 
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private SpawnInWaves _spawnWaves;
    [Space]
    [SerializeField] private StartGameScreen _startScreen;
    [SerializeField] private EndGameScreen _endScreen;
    
    private Health _playerHealth;

    private void OnEnable()
    {
        _startScreen.StartButtonClicked += OnStartButtonClicked;
        _endScreen.RestartButtonClicked += OnRestartButtonClicked;
    }

    private void OnDisable()
    {
        _startScreen.StartButtonClicked -= OnStartButtonClicked;
        _endScreen.RestartButtonClicked -= OnRestartButtonClicked;
    }

    private void Start()
    {
        _playerHealth = _player.GetComponent<Health>();
        _playerHealth.Death += OnPlayerDead;

        _enemySpawner.EnemyDead += OnEnemyDead;
        
        Time.timeScale = 0;
        _endScreen.Close();
        _startScreen.Open();
    }

    private void StartGame()
    {
        Time.timeScale = 1f;
        
        _player.transform.position = _playerStartPosition.position;
        _player.transform.rotation = _playerStartPosition.rotation;
        
        _player.Reset();
        
        _enemySpawner.ResetSpawner();
        _spawnWaves.ResetWaves();
    }

    private void OnPlayerDead()
    {
        _endScreen.SetTitleText(DefeatTitle);
        _endScreen.Open();
    }

    private void OnEnemyDead()
    {
        if (_spawnWaves.IsAllEnemiesSpawned() && _enemySpawner.GetActiveEnemiesCount() == 0)
        {
            _endScreen.SetTitleText(VictoryTitle);
            _endScreen.Open();
        }
    }

    private void OnStartButtonClicked()
    {
        _startScreen.Close();
        StartGame();
    }

    private void OnRestartButtonClicked()
    {
        _endScreen.Close();
        StartGame();
    }
}

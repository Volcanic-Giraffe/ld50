using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    [SerializeField] private LevelGenParams _nextWave;

    private int _waveNumber;

    public int WaveNumber => _waveNumber;
    
    void Start()
    {
        PanLevel.Instance.OnLevelStarted += () =>
        {
            SpawnNextWave();
        };

        PanLevel.Instance.OnEnemiesKilled += () =>
        {
            SpawnNextWave();
        };
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            SpawnNextWave();
        }
    }

    private void SpawnNextWave()
    {
        _waveNumber += 1;
        
        PanLevel.Instance.SpawnLevelWave(_nextWave);
        
        _nextWave.EnemiesMax += 1;
        _nextWave.EnemiesMin += 1;

        var pan = FindObjectOfType<Pan>();

        if (pan != null)
        {
            pan.IncreaseHeat();
        }
        
        GameStats.Instance.WavesDone = _waveNumber - 1;
    }

}

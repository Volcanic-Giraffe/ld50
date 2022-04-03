using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WavesManager : MonoBehaviour
{
    [SerializeField] private LevelGenParams _nextWave;

    [Header("Pan:")]
    [SerializeField] private float flipTimeMin;
    [SerializeField] private float flipTimeMax;

    [SerializeField] private float heatRiseMin;
    [SerializeField] private float heatRiseMax;

    private float heatTimer;
    private float flipTimer;
    
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
        
        heatTimer = Random.Range(heatRiseMin, heatRiseMax);
        flipTimer = Random.Range(flipTimeMin, flipTimeMax);
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            SpawnNextWave();
        }


        heatTimer -= Time.deltaTime;
        if (heatTimer <= 0)
        {
            heatTimer = Random.Range(heatRiseMin, heatRiseMax);
            PanLevel.Instance.Pan.IncreaseHeat();
        }
        
        
        flipTimer -= Time.deltaTime;
        if (flipTimer <= 0)
        {
            flipTimer = Random.Range(flipTimeMin, flipTimeMax);
            PanLevel.Instance.Pan.PanFlip();
        }
    }

    private void SpawnNextWave()
    {
        _waveNumber += 1;
        
        PanLevel.Instance.SpawnLevelWave(_nextWave);
        
        _nextWave.EnemiesMax += 1;
        _nextWave.EnemiesMin += 1;

        GameStats.Instance.WavesDone = _waveNumber - 1;
    }

}

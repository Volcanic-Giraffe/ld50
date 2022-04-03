using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;
    
    private void Awake()
    {
        Instance = FindObjectOfType<GameStats>();
    }

    public int KilledEnemies { get; set; }
    public int WavesDone { get; set; }
}

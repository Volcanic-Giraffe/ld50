using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Game Instance;
    
    [SerializeField] private AudioSource music;

    private bool _muted;

    private float _initialVolume;

    private void Awake()
    {
        Instance = FindObjectOfType<Game>();
        
        _initialVolume = music.volume;
    }

    private void ToggleMusic()
    {
        _muted = !_muted;
        music.DOFade(_muted ? 0 : _initialVolume, 0.1f);
        
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusic();
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sounds : MonoBehaviour
{
    public static Sounds Instance;
    
    public AudioSource aSource;

    public GameObject loopPlayerGO;

    public List<AudioClip> sounds;

    private Dictionary<string, AudioClip> _soundsByName;
    private Dictionary<string, List<AudioClip>> _soundsByPrefix;
    private Dictionary<string, AudioSource> _loopSounds;

    private Dictionary<string, bool> _loopPlaying;
    
    private void Awake()
    {
        Instance = FindObjectOfType<Sounds>();
        
        _soundsByPrefix = new Dictionary<string, List<AudioClip>>();
        _soundsByName = new Dictionary<string, AudioClip>();
        _loopSounds = new Dictionary<string, AudioSource>();
        _loopPlaying = new Dictionary<string, bool>();

        foreach (var sound in sounds)
        {
            _soundsByName.Add(sound.name, sound);
        }
    }

    public void PlayLoop(string soundName)
    {
        if (!_loopSounds.ContainsKey(soundName))
        {
            var loopAudioObj = Instantiate(loopPlayerGO, transform);

            var clip = _soundsByName[soundName];

            var loopAudio = loopAudioObj.GetComponent<AudioSource>();
            loopAudio.clip = clip;
            loopAudio.loop = true;

            _loopSounds.Add(soundName, loopAudio);
            _loopPlaying.Add(soundName, false);
        }

        var loop = _loopSounds[soundName];

        if (!loop.isPlaying)
        {
            loop.Play();
        }

        if (!_loopPlaying[soundName])
        {
            _loopPlaying[soundName] = true;
            loop.DOFade(1, 0.1f);
        }

    }

    public void StopLoop(string soundName)
    {
        if (_loopSounds.ContainsKey(soundName))
        {
            if (_loopPlaying[soundName])
            {
                _loopPlaying[soundName] = false;
                _loopSounds[soundName].DOFade(0, 0.1f);
            }
        }
    }

    public void StopAllLoops()
    {
        foreach (var loopSound in _loopSounds)
        {
            loopSound.Value.Stop();
            loopSound.Value.DOKill();
            _loopPlaying[loopSound.Key] = false;
        }
    }

    public void PlayExact(string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;
        
        var clip = _soundsByName[soundName];

        aSource.PlayOneShot(clip);
    }

    public void PlayRandom(string soundPrefix)
    {
        if (string.IsNullOrEmpty(soundPrefix)) return;
        
        if (!_soundsByPrefix.ContainsKey(soundPrefix))
        {
            var list = new List<AudioClip>();
            foreach (var sound in sounds)
            {
                if (sound.name.StartsWith(soundPrefix))
                {
                    list.Add(sound);
                }
            }

            _soundsByPrefix.Add(soundPrefix, list);
        }

        var clips = _soundsByPrefix[soundPrefix];
        var clip = clips[Random.Range(0, clips.Count)];

        aSource.PlayOneShot(clip);
    }

}
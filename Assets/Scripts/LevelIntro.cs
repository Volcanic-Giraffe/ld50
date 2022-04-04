using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelIntro : MonoBehaviour
{
    private Camera _camera;
    
    private Vector3 _originalCamRotation;

    private bool _introDone;
    
    private void Awake()
    {
        _camera = Camera.main;
        
        _originalCamRotation = _camera.transform.rotation.eulerAngles;
    }

    void Start()
    {
        _camera.transform.rotation = Quaternion.Euler(_originalCamRotation.x, -180f, _originalCamRotation.y);

    }

    public void BeginIntro(Action callback)
    {
        var uiDelay = 0.4f;
        
        _camera.transform.DORotate(_originalCamRotation, 1.3f).SetDelay(uiDelay).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }
    
}

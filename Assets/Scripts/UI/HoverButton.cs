using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleMod = 1.1f;
    
    private Vector3 _origScale;
    
    private void Awake()
    {
        _origScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_origScale * scaleMod, 0.1f);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_origScale, 0.1f);
    }
}

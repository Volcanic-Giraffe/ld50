using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleMod = 1.1f;
    [SerializeField] private float attentionDirection;
    
    private Vector3 _origScale;
    
    private float _origX;

    private RectTransform _rect;
    
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _origX = _rect.anchoredPosition.x;
        
        _origScale = transform.localScale;
    }

    private void Start()
    {
        if (attentionDirection != 0)
        {
            _rect.DOAnchorPosX(_origX + attentionDirection, 0.4f).SetLoops(-1, LoopType.Yoyo);
        }
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

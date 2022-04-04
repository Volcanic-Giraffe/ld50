using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectorUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private HeroPicker heroPicker;
    [SerializeField] private WeaponPicker weaponPicker;

    [SerializeField] private RectTransform startButtonHolder;
    [SerializeField] private Button startButton;
    private void Start()
    {
        
    }

    public void OnStartClicked()
    {
        Sounds.Instance.PlayRandom("click_a");
        PanLevel.Instance.BeginLevel(heroPicker.GetSelection(), weaponPicker.GetSelection());
        Hide();
    }

    public void Hide(bool instant = false)
    {
        if (instant)
        {
            container.gameObject.SetActive(false);
        }
        else
        {
            container.transform.DOMoveY(-1080f, 0.7f).SetEase(Ease.InOutBack);
        }
        
        startButton.interactable = false;
    }
}

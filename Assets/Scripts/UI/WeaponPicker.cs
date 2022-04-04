using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WeaponProfile
{
    public string Key;
    public Sprite Icon;
    public Sprite NameIcon;
    public string Name;
    public string Description;
    public GameObject WeaponGO;
}

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] private List<WeaponProfile> options;
    [SerializeField] private Image icon;
    [SerializeField] private Image nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private RectTransform buttonLeftHolder;
    [SerializeField] private RectTransform buttonRightHolder;

    private int _selected;

    private void Start()
    {
        UpdatePreview(false);
    }

    public void OnLeftClicked()
    {
        Sounds.Instance.PlayRandom("click_a");
        _selected -= 1;
        if (_selected < 0) _selected = options.Count - 1;
        
        UpdatePreview();
    }

    public void OnRightClicked()
    {
        Sounds.Instance.PlayRandom("click_a");
        _selected += 1;
        if (_selected >= options.Count) _selected = 0;

        UpdatePreview();
    }
    
    private void UpdatePreview(bool animate = true)
    {
        var option = options[_selected];

        icon.sprite = option.Icon;
        icon.SetNativeSize();
        
        nameText.sprite = option.NameIcon;
        nameText.SetNativeSize();
        descriptionText.SetText(option.Description);

        if (animate)
        {
            icon.transform.DOKill();
            icon.transform.localScale = Vector3.one * 0.9f;
            icon.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }
    
    public WeaponProfile GetSelection()
    {
        return options[_selected];
    }
}

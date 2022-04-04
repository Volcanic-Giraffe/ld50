using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HeroProfile
{
    public string Key;
    public Sprite Icon;
    public string Name;
    public string Description;
    public BodyDataSO BodyData;
}

public class HeroPicker : MonoBehaviour
{
    [SerializeField] private List<HeroProfile> options;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private RectTransform buttonLeftHolder;
    [SerializeField] private RectTransform buttonRightHolder;

    [SerializeField] private RectTransform iconBgRect;
    
    private int _selected;

    private float _iconBgOriginY;

    private void Awake()
    {
        _iconBgOriginY = iconBgRect.anchoredPosition.y;
    }

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
        
        nameText.SetText(option.Name);
        descriptionText.SetText(option.Description);

        if (animate)
        {
            iconBgRect.DOKill();
            iconBgRect.DOAnchorPosY(_iconBgOriginY - 10, 0.1f).OnComplete(() =>
            {
                iconBgRect.DOAnchorPosY(_iconBgOriginY, 0.1f);
            });

            icon.transform.DOKill();
            icon.transform.localScale = Vector3.one * 0.9f;
            icon.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
        
    }

    public HeroProfile GetSelection()
    {
        return options[_selected];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
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

    private int _selected;
    
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

    private void UpdatePreview()
    {
        var option = options[_selected];

        icon.sprite = option.Icon;
        icon.SetNativeSize();
        
        nameText.SetText(option.Name);
        descriptionText.SetText(option.Description);
    }

    public HeroProfile GetSelection()
    {
        return options[_selected];
    }
}

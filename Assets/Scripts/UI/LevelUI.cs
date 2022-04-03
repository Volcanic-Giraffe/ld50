using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;
    
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI ammoText;

    [SerializeField] private TextMeshProUGUI enemiesText;

    private void Awake()
    {
        Instance = FindObjectOfType<LevelUI>();
    }

    private void Start()
    {
        group.alpha = 0f;
        
        var player = PanLevel.Instance.Player;

        player.Damageable.OnHit += () =>
        {
            UpdateHp(player.Damageable.CurrentHealth, player.Damageable.MaxHealth);
        };
        player.Damageable.OnDie += () =>
        {
            UpdateHp(0, 0);
        };

        player.OnWeaponSwitched += OnWeaponSwitched;

        OnWeaponSwitched(player.Weapon);
    }

    private void OnWeaponSwitched(Weapon weapon)
    {
        weapon.OnClipUpdated += UpdateAmmo;
    }

    public void Show()
    {
        @group.alpha = 1f;
    }
    
    private void UpdateHp(float current, float max)
    {
        hpText.SetText($"hp: {current}/{max}");
        hpText.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f);
    }

    private void UpdateAmmo(int current, int max)
    {
        ammoText.SetText($"{current}/{max}");
        if (current == max || current == 0)
        {
            ammoText.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f);
        }

        ammoText.color = current == 0 ? Color.red : Color.white;
    }

    private void Update()
    {
        enemiesText.SetText($"Enemies: {PanLevel.Instance.Enemies.Count}");
    }

    public void ShowLevelUI()
    {
        group.alpha = 1;
    }
}

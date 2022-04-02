using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI ammoText;

    private void Start()
    {
        var player = PanLevel.Instance.Player;

        player.Damageable.OnHit += () =>
        {
            UpdateHp(player.Damageable.CurrentHealth, player.Damageable.MaxHealth);
        };
        player.Damageable.OnDie += () =>
        {
            UpdateHp(0, 0);
        };

        player.Weapon.OnClipUpdated += UpdateAmmo;
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
}

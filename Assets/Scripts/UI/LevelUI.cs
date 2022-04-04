using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;
    
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Image hpBar;
    [SerializeField] private RectTransform ammoPanel;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Image Reload;

    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI killedText;
    [SerializeField] private TextMeshProUGUI waveText;
    
    private void Awake()
    {
        Instance = FindObjectOfType<LevelUI>();
    }

    private void Start()
    {
        group.alpha = 0f;
        
        var player = PanLevel.Instance.Player;

        player.Damageable.OnHit += (hitInfo) =>
        {
            UpdateHp(player.Damageable.CurrentHealth, player.Damageable.MaxHealth);
        };
        player.Damageable.OnDie += () =>
        {
            UpdateHp(0, 0);
        };

        player.OnWeaponSwitched += OnWeaponSwitched;

        OnWeaponSwitched(player.Weapon, player.Weapon.BulletsInClip);
    }

    private void OnWeaponSwitched(Weapon weapon, int bullets)
    {
        Reload.gameObject.SetActive(false);

        weapon.OnClipUpdated += UpdateAmmo;
        weapon.OnReloadStart += Weapon_OnReloadStart;
        weapon.OnReloadEnd += Weapon_OnReloadEnd;
        UpdateAmmo(bullets, 0);
    }

    private void Weapon_OnReloadEnd()
    {
        Reload.DOKill();
        Reload.color = Color.white;
        Reload.gameObject.SetActive(false);
    }

    private void Weapon_OnReloadStart()
    {
        foreach (RectTransform child in ammoPanel)
        {
            Destroy(child.gameObject);
        }
        Reload.DOFade(0, 0.6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        Reload.gameObject.SetActive(true);
    }

    public void Show()
    {
        @group.alpha = 1f;
    }
    
    public void UpdateHp(float current, float max)
    {
        hpBar.fillAmount = current / max;
        hpBar.rectTransform.DOKill();
        hpBar.rectTransform.localScale = Vector3.one;
        hpBar.rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.3f);
    }

    public void setAmmoImage(GameObject image)
    {
        ammoPrefab = image;
    }
    private void UpdateAmmo(int current, int max = 0)
    {
        if (!ammoPrefab) return;

        if(ammoPanel.childCount > current)
        {
            var c = ammoPanel.GetChild(ammoPanel.childCount - 1);
            var rt = c.GetComponent<RectTransform>();
            rt.SetParent(ammoPanel.parent);
            c.GetComponent<Image>().DOFade(0, 0.2f);
            rt.DOPunchScale(Vector3.one * 0.1f, 0.3f).OnComplete(() => Destroy(rt.gameObject));
        } 
        else 
        {
            foreach (RectTransform child in ammoPanel)
            {
                child.DOKill();
                Destroy(child.gameObject);
            }
            for (int i = 0; i < current; i++)
            {
                var ammo = Instantiate(ammoPrefab);
                ammo.transform.SetParent(ammoPanel);
                var rt = ammo.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0, -i * rt.sizeDelta.y * 0.5f);
            }
        }
    }

    private void Update()
    {
        enemiesText.SetText($"Enemies: {PanLevel.Instance.Enemies.Count}");
        
        killedText.SetText($"Killed: {GameStats.Instance.KilledEnemies}");
        waveText.SetText($"Wave: {GameStats.Instance.WavesDone + 1}");
    }

    public void ShowLevelUI()
    {
        group.alpha = 1;
    }
}

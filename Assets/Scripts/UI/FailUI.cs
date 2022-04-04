using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FailUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Image fade;
    [SerializeField] private List<RectTransform> items;

    public void Awake()
    {
        container.gameObject.SetActive(false);
    }

    public void OnRestartClicked()
    {
        PanLevel.Instance.RestartLevel();
    }

    public void ShowAnimated()
    {
        container.gameObject.SetActive(true);

        StartCoroutine(AnimateIn());
    }

    private IEnumerator AnimateIn()
    {
        fade.color = Color.clear;
        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }
        
        yield return new WaitForSeconds(0.7f);
        fade.DOFade(0.6f, 0.3f);
        
        yield return new WaitForSeconds(0.7f);

        foreach (var item in items)
        {
            yield return new WaitForSeconds(0.7f);
            
            item.gameObject.SetActive(true);
            Sounds.Instance.PlayRandom("gun_01");
        }
    }
}

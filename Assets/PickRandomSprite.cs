using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PickRandomSprite : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    void Start()
    {
        Pick();
    }

    public void Pick()
    {
        if (sprites.Length > 0) GetComponent<SpriteRenderer>().sprite = sprites.PickRandom();
    }
}

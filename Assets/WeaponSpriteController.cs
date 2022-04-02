using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpriteController : MonoBehaviour
{
    [SerializeField] SpriteRenderer weaponSR;
    [SerializeField] SpriteRenderer muzzleSR;
    [SerializeField] Sprite muzzleFlashSide;
    [SerializeField] Sprite muzzleFlashForward;
    [SerializeField] Sprite weaponLeft;
    [SerializeField] Sprite weaponRight;
    [SerializeField] Sprite weaponForward;
    [SerializeField] Sprite weaponBackward;
    private GameObject rotateAnchor;

    public void SetParent(GameObject go)
    {
        rotateAnchor = go;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotateAnchor) return;
        var camVector = transform.position - Camera.main.transform.position;
        // 4 сектора, довернем на 45 для легкости расчетов
        var angle = Vector3.SignedAngle(camVector, transform.position - rotateAnchor.transform.position, Vector3.up) + 45;

        if (angle > 0) { 
            if(angle < 90)
            {
                weaponSR.sprite = weaponLeft;
            } else
            {
                weaponSR.sprite = weaponBackward;
            }
        } else {
            if (angle > -90)
            {
                weaponSR.sprite = weaponForward;
            }
            else
            {
                weaponSR.sprite = weaponRight;
            }
        }
    }
}

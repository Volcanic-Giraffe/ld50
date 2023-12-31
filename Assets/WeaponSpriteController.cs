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
    private float angle;

    public void SetParent(GameObject go)
    {
        rotateAnchor = go;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotateAnchor) return;
        var camVector = transform.position - Camera.main.transform.position;
        angle = Vector3.SignedAngle(camVector, transform.position - rotateAnchor.transform.position, Vector3.up);
        if (angle < 45 && angle > -45)
        {
            weaponSR.sprite = weaponBackward;
            muzzleSR.sprite = muzzleFlashForward;
            muzzleSR.transform.localScale = new Vector3(1, 1, 1);
        }
        if (angle > 45 && angle < 135)
        {
            weaponSR.sprite = weaponRight;
            muzzleSR.sprite = muzzleFlashSide;
            muzzleSR.transform.localScale = new Vector3(1,1,1);
        }
        if (angle > 135 || angle < -135) {
            weaponSR.sprite = weaponForward;
            muzzleSR.sprite = muzzleFlashForward;
            muzzleSR.transform.localScale = new Vector3(1, 1, 1);
        }
        if (angle < -45 && angle > -135)
        {
            weaponSR.sprite = weaponLeft;
            muzzleSR.sprite = muzzleFlashSide;
            muzzleSR.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}

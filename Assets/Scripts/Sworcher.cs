using DG.Tweening;
using UnityEngine;

public class Sworcher : MonoBehaviour
{
    void Start()
    {
        transform.DOPunchRotation(new Vector3(180f, 180f, 180f), 3f).SetLoops(-1, LoopType.Yoyo);
    }
}

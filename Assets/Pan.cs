using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pan : MonoBehaviour
{
    [SerializeField] private PanHeat heat;
    private float _originY;

    public PanHeat PanHeat => heat;
    private float intensity = 0.5f;
    
    private Rigidbody _rig;


    private void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _originY = transform.position.y;
    }
    
    public void IncreaseHeat()
    {
        PanHeat.SetRadius(PanHeat.Radius + 2);
        PanHeat.SetGlow(intensity);
        intensity += 0.5f;
        PanHeat.transform.DOMoveY(PanHeat.transform.position.y + 1, 2f);
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            PanFlip();
        }
    }

    public void PanFlip()
    {
        _rig.DOMoveY(_originY+ 3f, 1.4f).SetEase(Ease.InBack).OnComplete(
            () =>
            {
                FlipProps();
                
                _rig.DOMoveY(_originY, 0.7f).SetEase(Ease.OutBack);
            });
    }

    private void FlipProps()
    {
        var props =  PanLevel.Instance.Props;

        var force = 400f;

        foreach (var prop in props)
        {
            if (prop == null) continue;
            var rig = prop.GetComponent<Rigidbody>();

            if (rig != null)
            {
                rig.AddTorque(new Vector3(Random.Range(-force, force), Random.Range(-force, force), Random.Range(-force, force)));
            }
        }
    }
}

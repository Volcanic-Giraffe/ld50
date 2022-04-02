using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanLevel : MonoBehaviour
{
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private Transform bulletsContainer;
    
    public PanAimer Aimer { get; private set; }
    
    public static PanLevel Instance;

    public List<Bullet> Bullets { get; private set; }

    private void Awake()
    {
        Instance = FindObjectOfType<PanLevel>();

        Aimer = FindObjectOfType<PanAimer>();


        Bullets = new List<Bullet>();
    }

    public void SpawnBullet(BulletConfig config, Vector3 position, Quaternion rotation)
    {
        var bulletObj = Instantiate(config.Prefab, bulletsContainer);
        bulletObj.transform.position = position;

        bulletObj.transform.rotation = rotation;
        
        // bulletObj.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        
        var bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Setup(config);
        }
    }
    
}

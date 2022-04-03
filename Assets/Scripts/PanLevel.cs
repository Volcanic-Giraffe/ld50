using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PanLevel : MonoBehaviour
{
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private Transform bulletsContainer;
    [SerializeField] private Transform propsContainer;

    [Header("Level Gen:")]
    [SerializeField] private bool generateOnStart;
    [SerializeField] private LevelGenParams levelGenConfig;
    
    public PanAimer Aimer { get; private set; }
    
    public static PanLevel Instance;

    public PanHero Player { get; set; }
    
    public List<Transform> Boxes { get; private set; }
    public List<Bullet> Bullets { get; private set; }
    public List<Transform> Characters { get; private set; }

    private List<Vector3> _occupied;
    
    private const int PlaceAttempts = 100;

    private void Awake()
    {
        Instance = FindObjectOfType<PanLevel>();

        Aimer = FindObjectOfType<PanAimer>();
        
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PanHero>();
        
        Bullets = new List<Bullet>();
        Boxes = new List<Transform>();
        Characters = new List<Transform>();

        _occupied = new List<Vector3>();

        if (Player != null)
        {
            _occupied.Add(Player.transform.position);
        }
    }

    private void Start()
    {
        if (generateOnStart)
        {
            SpawnLevelWave();
        }
    }

    public void SpawnBullet(BulletConfig config, Vector3 position, Quaternion rotation)
    {
        var bulletObj = Instantiate(config.Prefab, bulletsContainer);
        bulletObj.transform.position = position;
        // bulletObj.transform.rotation = rotation;
        bulletObj.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        
        var bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Setup(config);
        }
    }
    
    public void SpawnLevelWave()
    {
        // enemies
        var enemiesCount = Random.Range(levelGenConfig.EnemiesMin, levelGenConfig.EnemiesMax);
        for (int i = 0; i < enemiesCount; i++)
        {
            var enemyGO =  levelGenConfig.EnemiesGO.PickRandom();
            var enemyObj = Instantiate(enemyGO, charactersContainer, true);
            PlaceWithAttempts(enemyObj, _occupied, levelGenConfig.SpawnRadius, levelGenConfig.EnemiesSpacing);
        }
        
        // props
        var propsCount = Random.Range(levelGenConfig.PropsMin, levelGenConfig.PropsMax);
        for (int i = 0; i < propsCount; i++)
        {
            var propGO =  levelGenConfig.PropsGO.PickRandom();
            var propObj = Instantiate(propGO, propsContainer, true);
            PlaceWithAttempts(propObj, _occupied, levelGenConfig.SpawnRadius, levelGenConfig.PropsSpacing);
        }
    }

    private void PlaceWithAttempts(GameObject item, List<Vector3> existing, float radius, float spacing)
    {
        for (int i = 0; i < PlaceAttempts; i++)
        {
            var point = RandomPoint(radius, 0f);

            var canPlace = existing.All(other => !(Vector3.Distance(point, other) < spacing));

            if (canPlace)
            {
                item.transform.localPosition = point;
                existing.Add(point);
                return;
            }
        }
        
        Destroy(item);
    }

    public Vector3 RandomPoint(float radius, float altitude = 0)
    {
        var angle = Random.value * Mathf.PI * 2f;
        var x = Mathf.Cos(angle) * radius;
        var z = Mathf.Sin(angle) * radius;

        return new Vector3(x, altitude, z);
    }
    
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            _occupied.Clear();
            SpawnLevelWave();
        }
    }
}

[Serializable]
public class LevelGenParams
{
    public int SpawnRadius;
    
    public List<GameObject> EnemiesGO;
    public int EnemiesMin;
    public int EnemiesMax;
    public float EnemiesSpacing;

    public List<GameObject> PropsGO;
    public int PropsMin;
    public int PropsMax;
    public float PropsSpacing;
}
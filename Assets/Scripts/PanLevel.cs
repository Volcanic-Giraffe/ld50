using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PanLevel : MonoBehaviour
{
    [SerializeField] private bool quickStart;

    [Header("Object containers:")]
    [SerializeField] private Transform charactersContainer;
    [SerializeField] private Transform bulletsContainer;
    [SerializeField] private Transform propsContainer;

    [Header("Level Gen:")]
    [SerializeField] private bool generateOnStart;
    [SerializeField] private LevelGenParams levelGenConfig;
    
    public PanAimer Aimer { get; private set; }
    
    public static PanLevel Instance;

    public PanHero Player { get; set; }
    public Pan Pan { get; set; }
    
    public List<Transform> Props { get; private set; }
    public List<Bullet> Bullets { get; private set; }
    public List<AiInput> Enemies { get; private set; }

    private List<Vector3> _occupied;
    
    private const int PlaceAttempts = 100;

    public event Action OnLevelStarted;
    public event Action OnEnemiesKilled;

    public bool Started { get; set; }
    public bool Failed { get; set; }

    private void Awake()
    {
        Instance = FindObjectOfType<PanLevel>();

        Aimer = FindObjectOfType<PanAimer>();
        
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PanHero>();
        Pan = FindObjectOfType<Pan>();
        
        Bullets = new List<Bullet>();
        Props = new List<Transform>();
        Enemies = new List<AiInput>();

        _occupied = new List<Vector3>();
    }
    private void Start()
    {
        if (quickStart)
        {
            FindObjectOfType<SelectorUI>().Hide(true);
            PrepareLevel(null, null);
            BeginLevel();
        }

    }

    private void Update()
    {
        if (Failed && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        
        if (Input.GetButtonDown("Cancel"))
        {
            if (Started)
            {
                RestartLevel();
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void BeginIntro(HeroProfile pickedHero, WeaponProfile pickedGun)
    {
        PrepareLevel(pickedHero, pickedGun);
        
        GetComponent<LevelIntro>().BeginIntro(() =>
        {
            BeginLevel();
        });
    }

    public void PrepareLevel(HeroProfile pickedHero, WeaponProfile pickedGun)
    {
        var ui = FindObjectOfType<LevelUI>();
        if (pickedHero != null)
        {
            Player.Intro();
            Player.ChangeBody(pickedHero.BodyData);
            ui.UpdateHp(Player.Damageable.CurrentHealth, Player.Damageable.MaxHealth);
        }
        if (pickedGun != null)
        {
            ui.setAmmoImage(pickedGun.BulletUIcon);
            Player.ChangeWeapon(pickedGun.WeaponGO);
        }
    }
    
    public void BeginLevel()
    {
        LevelUI.Instance.ShowLevelUI();
        
        FindExistingEnemies();
        FindExistingProps();
        
        if (generateOnStart)
        {
            SpawnLevelWave(levelGenConfig);
        }

        Player.Damageable.OnDie += LevelFailed;
        
        OnLevelStarted?.Invoke();

        Started = true;
    }

    private void LevelFailed()
    {
        Started = false;
        Failed = true;
        
        FindObjectOfType<FailUI>().ShowAnimated();
    }

    private void FindExistingProps()
    {
        var props = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var prop in props)
        {
            if (prop.GetComponent<Rigidbody>() != null)
            {
                Props.Add(prop.transform);
            }
        }
    }

    private void FindExistingEnemies()
    {
        var foundEnemies = FindObjectsOfType<AiInput>();

        foreach (var enemy in foundEnemies)
        {
            AddExistingEnemy(enemy);
        }
    }

    public void AddExistingEnemy(AiInput enemy)
    {
        if (Enemies.Contains(enemy)) return;
        
        Enemies.Add(enemy);

        enemy.Damageable.OnDie += () =>
        {
            Enemies.Remove(enemy);

            CheckAllKilled();
        };
    }

    private void CheckAllKilled()
    {
        if (Enemies.Count == 0) OnEnemiesKilled?.Invoke();
    }

    public void SpawnBullet(BulletConfig config, Vector3 position, Quaternion rotation, int team)
    {
        var bulletObj = Instantiate(config.Prefab, bulletsContainer);
        bulletObj.transform.position = position;
        // bulletObj.transform.rotation = rotation;
        bulletObj.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        
        var bullet = bulletObj.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Setup(config, team);
        }
    }
    
    public void SpawnLevelWave(LevelGenParams config)
    {
        ResetOccupied();
        
        // enemies
        var enemiesCount = Random.Range(config.EnemiesMin, config.EnemiesMax);
        if (enemiesCount > config.EnemiesLimit) enemiesCount = config.EnemiesLimit;
        
        for (int i = 0; i < enemiesCount; i++)
        {
            var enemyGO =  config.EnemiesGO.PickRandom();
            var enemyObj = Instantiate(enemyGO, charactersContainer, true);
            if (PlaceWithAttempts(enemyObj, _occupied, config.SpawnRadius, config.EnemiesSpacing))
            {
                var enemyComp = enemyObj.GetComponent<AiInput>();

                if (enemyComp != null)
                {
                    AddExistingEnemy(enemyComp);
                }
            }
        }
        
        // props
        var propsCount = Random.Range(config.PropsMin, config.PropsMax);
        for (int i = 0; i < propsCount; i++)
        {
            var propGO =  config.PropsGO.PickRandom();
            var propObj = Instantiate(propGO, propsContainer, true);
            PlaceWithAttempts(propObj, _occupied, config.SpawnRadius, config.PropsSpacing);
            
            Props.Add(propObj.transform);
        }
    }

    private void ResetOccupied()
    {
        _occupied.Clear();
        
        // tbd: add existing boxes
        
        if (Player != null)
        {
            _occupied.Add(Player.transform.position);
        }
    }

    private bool PlaceWithAttempts(GameObject item, List<Vector3> existing, float radius, float spacing)
    {
        for (int i = 0; i < PlaceAttempts; i++)
        {
            var point = RandomPoint(radius, 3f);

            var canPlace = existing.All(other => !(Vector3.Distance(point, other) < spacing));

            if (canPlace)
            {
                item.transform.localPosition = point;
                existing.Add(point);
                return true;
            }
        }
        
        Destroy(item);
        return false;
    }

    public Vector3 RandomPoint(float radius, float altitude = 0)
    {
        var angle = Random.value * Mathf.PI * 2f;
        var x = Mathf.Cos(angle) * radius;
        var z = Mathf.Sin(angle) * radius;

        return new Vector3(x, altitude, z);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

[Serializable]
public class LevelGenParams
{
    public int SpawnRadius;
    
    public List<GameObject> EnemiesGO;
    public int EnemiesMin;
    public int EnemiesMax;
    public int EnemiesLimit;
    public float EnemiesSpacing;

    public List<GameObject> PropsGO;
    public int PropsMin;
    public int PropsMax;
    public float PropsSpacing;
}
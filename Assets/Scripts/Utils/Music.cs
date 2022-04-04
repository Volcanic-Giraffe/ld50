using DG.Tweening;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance;

    [SerializeField] private AudioSource music;

    private bool _muted;

    private float _initialVolume;

    private void Awake()
    {
        Instance = FindObjectOfType<Music>();
        
        _initialVolume = music.volume;
    }

    private void ToggleMusic()
    {
        _muted = !_muted;
        music.DOFade(_muted ? 0 : _initialVolume, 0.1f);
        
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusic();
        }
    }
}

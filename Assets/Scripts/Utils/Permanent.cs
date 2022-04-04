using UnityEngine;

public class Permanent : MonoBehaviour
{
    public static Permanent Instance;

    private void Awake()
    {
        Instance = FindObjectOfType<Permanent>();
        DontDestroyOnLoad(gameObject);
    }

}

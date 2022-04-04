using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject permanentGO;

    private void Awake()
    {
        if (FindObjectsOfType<Permanent>().Length == 0)
        {
            Instantiate(permanentGO);
        }
    }
}

using UnityEngine;

public class WallsProc : MonoBehaviour
{
    [SerializeField] private GameObject wallGO;
    [SerializeField] private int count;
    [SerializeField] private float radius;

    private void Awake()
    {
        for (int i = 0; i < count; i++)
        {
            var wall = Instantiate(wallGO, transform);
            
            var angle = (i/(count-1f)) * Mathf.PI * 2f;
            var x = Mathf.Cos(angle) * radius;
            var z = Mathf.Sin(angle) * radius;

            wall.transform.position = new Vector3(x, 0, z);
            wall.transform.LookAt(transform.position);
        }
    }
}

using UnityEngine;

public class GGProjectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<ArkBlock>())
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTL : MonoBehaviour
{
    public float TimeToLive=3;
   

    // Update is called once per frame
    void Update()
    {
        TimeToLive -= Time.deltaTime;
        if(TimeToLive <=0)
        {
            Destroy(gameObject);
        }
    }
}

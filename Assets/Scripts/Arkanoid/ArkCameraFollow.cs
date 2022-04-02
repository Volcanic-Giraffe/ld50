using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkCameraFollow : MonoBehaviour
{
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            var pos = transform.position;

            transform.position = new Vector3(pos.x, Target.transform.position.y + 1.5f, pos.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target;
    public Vector3 position;

    void LateUpdate()
    {
        if (CameraShake.Instance.shakeDur <= 0)
        {
            Vector3 newPos = position + target.transform.position;
            transform.position = newPos;
        }
    }
}

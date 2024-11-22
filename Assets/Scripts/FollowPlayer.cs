using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target;
    public Vector3 position;
    public float smoothSpeed = 0.5f;

    void LateUpdate()
    {
        Vector3 newPos = position + target.transform.position;
        transform.position = newPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMotion : MonoBehaviour
{
    public Transform target;
    private Vector3 origin;
    private void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = origin + target.position;
    }
}

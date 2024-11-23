using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    public float shakeDur = 0f;
    public float shakeMag = 0.6f;
    public float dampingSpeed = 1f;

    private Vector3 initialPos;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        initialPos = transform.localPosition;
        if (shakeDur > 0)
        {
            transform.localPosition = initialPos + Random.insideUnitSphere * shakeMag;
            shakeDur -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDur = 0;
            transform.localPosition = initialPos;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Shake();
        }
    }

    public void Shake()
    {
        shakeDur = .1f;
    }
}

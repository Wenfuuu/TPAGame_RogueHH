using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light lightSource;
    float originalIntensity;
    public float flickerAmount = 1f;
    public float flickerSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        originalIntensity = lightSource.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        float flicker = Random.Range(-flickerAmount, flickerAmount);
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, originalIntensity + flicker, flickerSpeed);
    }
}

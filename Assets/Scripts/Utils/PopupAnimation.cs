using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve heightCurve;

    private TextMeshProUGUI text;
    private float time = 0;
    private Vector3 origin;

    private int random;

    void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
        random = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, opacityCurve.Evaluate(time));
        if(random == 0) transform.position = origin + new Vector3(0 + heightCurve.Evaluate(time), 1 + heightCurve.Evaluate(time), 0);
        else transform.position = origin + new Vector3(0 - heightCurve.Evaluate(time), 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
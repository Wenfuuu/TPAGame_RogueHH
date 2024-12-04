using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopupAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve heightCurve;

    private TextMeshProUGUI text;
    private float time = 0;
    private Vector3 origin;
    private string poolKey;

    private int random;

    void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        random = Random.Range(0, 2);
    }

    public void Initialize(Vector3 newOrigin, string key)
    {
        origin = newOrigin;
        poolKey = key; 
        time = 0;
        gameObject.SetActive(true); 
    }

    void Update()
    {
        if (time > opacityCurve.keys[opacityCurve.length - 1].time)
        {
            StartCoroutine(ReturnToPool());
            return;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, opacityCurve.Evaluate(time));
        if (random == 0)
            transform.position = origin + new Vector3(0 + heightCurve.Evaluate(time), 1 + heightCurve.Evaluate(time), 0);
        else
            transform.position = origin + new Vector3(0 - heightCurve.Evaluate(time), 1 + heightCurve.Evaluate(time), 0);

        time += Time.deltaTime;
    }

    private IEnumerator ReturnToPool()
    {
        yield return null; 
        ObjectPool.EnqueueObject(gameObject, poolKey);
    }
}

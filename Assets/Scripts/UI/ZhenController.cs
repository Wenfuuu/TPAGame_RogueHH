using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZhenController : MonoBehaviour
{
    public TextMeshProUGUI ZhenText;

    public IntEventChannel UpdateZhen;

    private void OnEnable()
    {
        UpdateZhen.OnEventRaised += UpdateZhenText;
    }

    private void OnDisable()
    {
        UpdateZhen.OnEventRaised -= UpdateZhenText;
    }

    private void UpdateZhenText(int zhen)
    {
        ZhenText.text = zhen.ToString();
    }
}

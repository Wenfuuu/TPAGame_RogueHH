using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameGenerator : MonoBehaviour
{
    public Text nameText;

    private string[] _names =
    {
        "AC",
        "AS",
        "BD",
        "BT",
        "CT",
        "FO",
        "GN",
        "GY",
        "HO",
        "KH",
        "MM",
        "MR",
        "MV",
        "NS",
        "OV",
        "PL",
        "RU",
        "TI",
        "VD",
        "VM",
        "WS",
        "WW",
        "YD"
    };

    // Start is called before the first frame update
    void Start()
    {
        int idx = Random.Range(0, _names.Length);
        nameText.text = _names[idx];
    }
}

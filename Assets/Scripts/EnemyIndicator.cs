using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    public GameObject alertIndicator;
    public GameObject aggroIndicator;

    public void OnAlert()
    {
        Debug.Log("enabling alert indicator");
        alertIndicator.SetActive(true);
    }

    public void OffAlert()
    {
        alertIndicator.SetActive(false);
    }

    public void OnAggro()
    {
        OffAlert();
        aggroIndicator.SetActive(true);
    }
}

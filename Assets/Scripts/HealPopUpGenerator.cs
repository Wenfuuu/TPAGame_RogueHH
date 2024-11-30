using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealPopUpGenerator : MonoBehaviour
{
    public static HealPopUpGenerator Instance;
    public GameObject Heal;

    private void Start()
    {
        Instance = this;
    }

    public void CreatePopUp(int healAmount, Vector3 position)
    {
        position.y = 2.75f;
        var popup = Instantiate(Heal, position, Quaternion.identity);
        TextMeshProUGUI temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = healAmount.ToString();

        Destroy(popup, 1f);
    }
}

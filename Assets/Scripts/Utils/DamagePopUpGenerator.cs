using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator Instance;
    public GameObject NormalDamage;
    public GameObject CritDamage;

    private void Start()
    {
        Instance = this;
    }

    public void CreatePopUp(int damage, bool isCritical, Vector3 position)
    {
        GameObject prefab;
        if (isCritical)
        {
            Debug.Log("generating crit popup");
            prefab = CritDamage;
        }
        else prefab = NormalDamage;

        position.y = 2.75f;
        var popup = Instantiate(prefab, position, Quaternion.identity);
        TextMeshProUGUI temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = damage.ToString();

        Destroy(popup, 1f);
    }
}

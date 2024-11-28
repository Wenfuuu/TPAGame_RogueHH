using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator Instance;
    public GameObject prefab;

    private void Start()
    {
        Instance = this;
    }

    public void CreatePopUp(int damage, bool isCritical, Vector3 position)
    {
        position.y = 2.75f;
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = damage.ToString();

        Destroy(popup, 1f);
    }
}

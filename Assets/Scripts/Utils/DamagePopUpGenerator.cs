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

        ObjectPool.poolLookup = new Dictionary<string, GameObject>();
        ObjectPool.poolDictionary = new Dictionary<string, Queue<GameObject>>();
        ObjectPool.SetupPool(NormalDamage, 10, "NormalDamage");
        ObjectPool.SetupPool(CritDamage, 5, "CritDamage");
    }

    public void CreatePopUp(int damage, bool isCritical, Vector3 position)
    {
        //GameObject prefab;
        //if (isCritical)
        //{
        //    Debug.Log("generating crit popup");
        //    prefab = CritDamage;
        //}
        //else prefab = NormalDamage;

        //position.y = 2.75f;
        //var popup = Instantiate(prefab, position, Quaternion.identity);
        //TextMeshProUGUI temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //temp.text = damage.ToString();

        //Destroy(popup, 1f);

        string poolKey = isCritical ? "CritDamage" : "NormalDamage";
        GameObject popup = ObjectPool.DequeueObject(poolKey);
        if (popup != null)
        {
            var popupScript = popup.GetComponent<DamagePopupAnimation>();
            popupScript.Initialize(position + new Vector3(0, 1.75f, 0), poolKey);

            TextMeshProUGUI temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            temp.text = damage.ToString();
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject popup, string poolKey, float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.EnqueueObject(popup, poolKey);
    }
}

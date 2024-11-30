using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelPopUpGenerator : MonoBehaviour
{
    public static LevelPopUpGenerator Instance;
    public GameObject LevelUp;

    private void Start()
    {
        Instance = this;
    }

    public void CreatePopUp(Vector3 position)
    {
        position.y = 2.75f;
        var popup = Instantiate(LevelUp, position, Quaternion.identity);
        Destroy(popup, 1f);
    }
}

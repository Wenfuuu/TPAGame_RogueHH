using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDetailSO", menuName = "Upgrade/Upgrade Detail")]
public class UpgradeDetailSO : ScriptableObject
{
    public Sprite image;
    public string title;
    public string description;
    public string upgrade;// ex: +2 def
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cheatcode : MonoBehaviour
{
    public TMP_InputField inputField;
    public PlayerStatsSO playerStats;
    public IntEventChannel UpdateZhen;
    public EventChannel UnlockFloor;

    private string[] codes =
    {
        "tpagamegampang",
        "hesoyam",
        "opensesame"
    };

    public void ApplyCode(string code)
    {
        if(code == "tpagamegampang")// increase 20k zhen
        {
            playerStats.IncreaseZhen(20000);
            UpdateZhen.RaiseEvent(playerStats.Zhen);
        }
        else if(code == "hesoyam")// increase 5k exp
        {
            playerStats.IncreaseEXP(5000);
            if(playerStats.CurrentEXP >= playerStats.MaxEXP) playerStats.LevelUp();
            //raise event to change stats in upgrade

        }
        else// increase unlocked floor to 101
        {
            playerStats.UnlockAllFloor();
            UnlockFloor.RaiseEvent();
        }
    }

    public void CheckInput()
    {
        //Debug.Log("checking cheat code");
        string inputText = inputField.text.ToLower();
        Debug.Log(inputText);

        foreach (string code in codes)
        {
            if (inputText == code)
            {
                ApplyCode(code);
                //Debug.Log("cheat code found");
                SFXManager.Instance.PlaySFX(Sounds.Instance.CheatCodeSFX, transform, 1f);
                ResetInputField();
                return;
            }
        }
    }

    private void ResetInputField()
    {
        inputField.text = "";
        inputField.ActivateInputField();
    }
}

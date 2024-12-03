using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeMenu : MonoBehaviour
{
    public AudioClip sceneBGM;
    public PlayerStatsSO playerStats;
    public IntEventChannel UpdateZhen;
    public IntEventChannel UpgradePrice;

    public EventChannel UpgradeHPDetail;
    public IntEventChannel UpgradeHPStats;
    public IntEventChannel UpdateHPLevel;

    public EventChannel UpgradeAtkDetail;
    public IntEventChannel UpgradeAtkStats;
    public IntEventChannel UpdateAtkLevel;

    public EventChannel UpgradeDefDetail;
    public IntEventChannel UpgradeDefStats;
    public IntEventChannel UpdateDefLevel;

    public EventChannel UpgradeCRDetail;
    public IntEventChannel UpgradeCRStats;
    public IntEventChannel UpdateCRLevel;

    public EventChannel UpgradeCDDetail;
    public IntEventChannel UpgradeCDStats;
    public IntEventChannel UpdateCDLevel;

    public BoolEventChannel ShowAlert;

    private void Start()
    {
        if (sceneBGM != null)
        {
            BGMManager.Instance.PlayBGM(sceneBGM);
            BGMManager.Instance.FadeInBGM();
        }

        //raise event for UI (anggap script ini kyk player di game)
        UpdateZhen.RaiseEvent(playerStats.Zhen);
        UpdateHPLevel.RaiseEvent(playerStats.HPLevel);
        UpdateAtkLevel.RaiseEvent(playerStats.AtkLevel);
        UpdateDefLevel.RaiseEvent(playerStats.DefLevel);
        UpdateCRLevel.RaiseEvent(playerStats.CRLevel);
        UpdateCDLevel.RaiseEvent(playerStats.CDLevel);
    }

    public void ShowUpgradeHP()// klik image kiri
    {
        UpgradeHPDetail.RaiseEvent();
        UpgradeHPStats.RaiseEvent(playerStats.MaxHP);
        int price = 10 + playerStats.HPLevel * 40 + playerStats.TotalUpgrade * 10;
        UpgradePrice.RaiseEvent(price);
        ShowAlert.RaiseEvent(false);
    }

    public void UpgradeHP()// klik upgrade button
    {
        int price = 10 + playerStats.HPLevel * 40 + playerStats.TotalUpgrade * 10;//itung price
        Debug.Log("upgrading hp with price :" + price);
        //validasi kalo zhen ga cukup
        if(playerStats.Zhen < price)
        {
            //set active "you dont have enough zhen"
            ShowAlert.RaiseEvent(true);
            return;
        }else if(playerStats.HPLevel >= 45)
        {
            return;
        }
        //kurangin zhen
        playerStats.DecreaseZhen(price);
        //add sfx
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.PurchaseSFX, transform, 1f);
        //naikin stat + level
        playerStats.MaxHP += 10;
        playerStats.HPLevel++;
        playerStats.TotalUpgrade++;
        //raise event untuk UI
        UpdateZhen.RaiseEvent(playerStats.Zhen);
        UpgradeHPStats.RaiseEvent(playerStats.MaxHP);
        price = 10 + playerStats.HPLevel * 40 + playerStats.TotalUpgrade * 10;
        UpgradePrice.RaiseEvent(price);
        //sisa event utk buat Lvl1/45
        UpdateHPLevel.RaiseEvent(playerStats.HPLevel);
    }

    public void ShowUpgradeAtk()
    {
        UpgradeAtkDetail.RaiseEvent();
        UpgradeAtkStats.RaiseEvent(playerStats.Attack);
        int price = 10 + playerStats.AtkLevel * 40 + playerStats.TotalUpgrade * 10;
        UpgradePrice.RaiseEvent(price);
        ShowAlert.RaiseEvent(false);
    }

    public void UpgradeAtk()
    {
        int price = 10 + playerStats.AtkLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        Debug.Log("upgrading atk with price :" + price);
        if (playerStats.Zhen < price)
        {
            ShowAlert.RaiseEvent(true);
            return;
        }
        else if (playerStats.AtkLevel >= 45)//ganti
        {
            return;
        }
        playerStats.DecreaseZhen(price);
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.PurchaseSFX, transform, 1f);
        playerStats.Attack += 2;//ganti
        playerStats.AtkLevel++;//ganti
        playerStats.TotalUpgrade++;
        UpdateZhen.RaiseEvent(playerStats.Zhen);
        UpgradeAtkStats.RaiseEvent(playerStats.Attack);//ganti
        price = 10 + playerStats.AtkLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        UpgradePrice.RaiseEvent(price);
        UpdateAtkLevel.RaiseEvent(playerStats.AtkLevel);//ganti
    }

    public void ShowUpgradeDef()
    {
        UpgradeDefDetail.RaiseEvent();
        UpgradeDefStats.RaiseEvent(playerStats.Defense);
        int price = 10 + playerStats.DefLevel * 40 + playerStats.TotalUpgrade * 10;
        UpgradePrice.RaiseEvent(price);
        ShowAlert.RaiseEvent(false);
    }

    public void UpgradeDef()
    {
        int price = 10 + playerStats.DefLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        if (playerStats.Zhen < price)
        {
            //set active "you dont have enough zhen"
            ShowAlert.RaiseEvent(true);
            return;
        }
        else if (playerStats.DefLevel >= 45)//ganti
        {
            return;
        }
        playerStats.DecreaseZhen(price);
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.PurchaseSFX, transform, 1f);
        playerStats.Defense += 2;//ganti
        playerStats.DefLevel++;//ganti
        playerStats.TotalUpgrade++;
        UpdateZhen.RaiseEvent(playerStats.Zhen);
        UpgradeDefStats.RaiseEvent(playerStats.Defense);//ganti
        price = 10 + playerStats.DefLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        UpgradePrice.RaiseEvent(price);
        UpdateDefLevel.RaiseEvent(playerStats.DefLevel);//ganti
    }

    public void ShowUpgradeCR()
    {
        UpgradeCRDetail.RaiseEvent();
        UpgradeCRStats.RaiseEvent(playerStats.CritRate);
        int price = 10 + playerStats.CRLevel * 40 + playerStats.TotalUpgrade * 10;
        UpgradePrice.RaiseEvent(price);
        ShowAlert.RaiseEvent(false);
    }

    public void UpgradeCR()
    {
        int price = 10 + playerStats.CRLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        if (playerStats.Zhen < price)
        {
            //set active "you dont have enough zhen"
            ShowAlert.RaiseEvent(true);
            return;
        }
        else if (playerStats.CRLevel >= 45)//ganti
        {
            return;
        }
        playerStats.DecreaseZhen(price);
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.PurchaseSFX, transform, 1f);
        playerStats.CritRate += 2;//ganti
        playerStats.CRLevel++;//ganti
        playerStats.TotalUpgrade++;
        UpdateZhen.RaiseEvent(playerStats.Zhen);
        UpgradeCRStats.RaiseEvent(playerStats.CritRate);//ganti 2
        price = 10 + playerStats.CRLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        UpgradePrice.RaiseEvent(price);
        UpdateCRLevel.RaiseEvent(playerStats.CRLevel);//ganti 2
    }

    public void ShowUpgradeCD()
    {
        UpgradeCDDetail.RaiseEvent();
        UpgradeCDStats.RaiseEvent(playerStats.CritDamage);
        int price = 10 + playerStats.CDLevel * 40 + playerStats.TotalUpgrade * 10;
        UpgradePrice.RaiseEvent(price);
        ShowAlert.RaiseEvent(false);
    }

    public void UpgradeCD()
    {
        int price = 10 + playerStats.CDLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        if (playerStats.Zhen < price)
        {
            //set active "you dont have enough zhen"
            ShowAlert.RaiseEvent(true);
            return;
        }
        else if (playerStats.CDLevel >= 45)//ganti
        {
            return;
        }
        playerStats.DecreaseZhen(price);
        SFXManager.Instance.PlayRandomSFX(Sounds.Instance.PurchaseSFX, transform, 1f);
        playerStats.CritDamage += 2;//ganti
        playerStats.CDLevel++;//ganti
        playerStats.TotalUpgrade++;
        UpdateZhen.RaiseEvent(playerStats.Zhen);
        UpgradeCDStats.RaiseEvent(playerStats.CritDamage);//ganti 2
        price = 10 + playerStats.CDLevel * 40 + playerStats.TotalUpgrade * 10;//ganti
        UpgradePrice.RaiseEvent(price);
        UpdateCDLevel.RaiseEvent(playerStats.CDLevel);//ganti 2
    }

    private IEnumerator TransitionBGM(AudioClip clip)
    {
        BGMManager.Instance.FadeOutBGM();

        yield return new WaitForSeconds(BGMManager.Instance.fadeDuration);

        BGMManager.Instance.PlayBGM(clip);
        BGMManager.Instance.FadeInBGM();
    }

    private IEnumerator TransitionToScene(int buildidx)
    {
        BGMManager.Instance.FadeOutBGM();

        yield return new WaitForSeconds(BGMManager.Instance.fadeDuration);

        SceneManager.LoadScene(buildidx);
    }

    public void LoadScene(int buildidx)
    {
        StartCoroutine(TransitionToScene(buildidx));
    }

    public void PlayGame()
    {
        LoadScene(2);
    }

    public void ExitToMenu()
    {
        LoadScene(0);
    }
}

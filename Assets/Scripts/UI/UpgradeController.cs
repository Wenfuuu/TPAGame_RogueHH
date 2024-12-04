using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    //SO
    public UpgradeDetailSO hpDetail;
    public UpgradeDetailSO atkDetail;
    public UpgradeDetailSO defDetail;
    public UpgradeDetailSO crDetail;
    public UpgradeDetailSO cdDetail;

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

    public Image imageIcon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI currentStats;// int
    public TextMeshProUGUI upgradeEffect;
    public TextMeshProUGUI priceText;// int

    public TextMeshProUGUI HPLevelText;
    public TextMeshProUGUI AtkLevelText;
    public TextMeshProUGUI DefLevelText;
    public TextMeshProUGUI CRLevelText;
    public TextMeshProUGUI CDLevelText;

    public TextMeshProUGUI AlertText;

    public Button upgradeBtn;
    public UpgradeMenu menu;

    public GameObject detail;// untuk set active
    public GameObject alert;

    private bool IsActive = false;

    private void OnEnable()
    {
        UpgradePrice.OnEventRaised += ShowPrice;

        UpgradeHPDetail.OnEventRaised += ShowHPDetail;
        UpgradeHPStats.OnEventRaised += ShowHPStats;
        UpdateHPLevel.OnEventRaised += UpdateHPTextLevel;

        UpgradeAtkDetail.OnEventRaised += ShowAtkDetail;
        UpgradeAtkStats.OnEventRaised += ShowAtkStats;
        UpdateAtkLevel.OnEventRaised += UpdateAtkTextLevel;

        UpgradeDefDetail.OnEventRaised += ShowDefDetail;
        UpgradeDefStats.OnEventRaised += ShowDefStats;
        UpdateDefLevel.OnEventRaised += UpdateDefTextLevel;

        UpgradeCRDetail.OnEventRaised += ShowCRDetail;
        UpgradeCRStats.OnEventRaised += ShowCRStats;
        UpdateCRLevel.OnEventRaised += UpdateCRTextLevel;

        UpgradeCDDetail.OnEventRaised += ShowCDDetail;
        UpgradeCDStats.OnEventRaised += ShowCDStats;
        UpdateCDLevel.OnEventRaised += UpdateCDTextLevel;

        ShowAlert.OnEventRaised += ShowAlertText;
    }

    private void OnDisable()
    {
        UpgradePrice.OnEventRaised -= ShowPrice;

        UpgradeHPDetail.OnEventRaised -= ShowHPDetail;
        UpgradeHPStats.OnEventRaised -= ShowHPStats;
        UpdateHPLevel.OnEventRaised -= UpdateHPTextLevel;

        UpgradeAtkDetail.OnEventRaised -= ShowAtkDetail;
        UpgradeAtkStats.OnEventRaised -= ShowAtkStats;
        UpdateAtkLevel.OnEventRaised -= UpdateAtkTextLevel;

        UpgradeDefDetail.OnEventRaised -= ShowDefDetail;
        UpgradeDefStats.OnEventRaised -= ShowDefStats;
        UpdateDefLevel.OnEventRaised -= UpdateDefTextLevel;

        UpgradeCRDetail.OnEventRaised -= ShowCRDetail;
        UpgradeCRStats.OnEventRaised -= ShowCRStats;
        UpdateCRLevel.OnEventRaised -= UpdateCRTextLevel;

        UpgradeCDDetail.OnEventRaised -= ShowCDDetail;
        UpgradeCDStats.OnEventRaised -= ShowCDStats;
        UpdateCDLevel.OnEventRaised -= UpdateCDTextLevel;

        ShowAlert.OnEventRaised -= ShowAlertText;
    }

    private void ShowAlertText(bool value)
    {
        alert.SetActive(value);
    }

    private void UpdateHPTextLevel(int level)
    {
        HPLevelText.text = $"Lvl. {level}/45";
    }

    private void UpdateAtkTextLevel(int level)
    {
        AtkLevelText.text = $"Lvl. {level}/45";
    }

    private void UpdateDefTextLevel(int level)
    {
        DefLevelText.text = $"Lvl. {level}/45";
    }

    private void UpdateCRTextLevel(int level)
    {
        CRLevelText.text = $"Lvl. {level}/45";
    }

    private void UpdateCDTextLevel(int level)
    {
        CDLevelText.text = $"Lvl. {level}/45";
    }

    private void SetActive()
    {
        if (IsActive) return;
        IsActive = true;
        detail.SetActive(true);
        alert.SetActive(false);
    }

    private void ShowHPDetail()
    {
        SetActive();
        imageIcon.sprite = hpDetail.image;
        title.text = hpDetail.title;
        description.text = hpDetail.description;
        upgradeEffect.text = "Upgrade: " + hpDetail.upgrade;

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(() => menu.UpgradeHP());
    }

    private void ShowAtkDetail()
    {
        SetActive();
        imageIcon.sprite = atkDetail.image;
        title.text = atkDetail.title;
        description.text = atkDetail.description;
        upgradeEffect.text = "Upgrade: " + atkDetail.upgrade;

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(() => menu.UpgradeAtk());
    }

    private void ShowDefDetail()
    {
        SetActive();
        imageIcon.sprite = defDetail.image;
        title.text = defDetail.title;
        description.text = defDetail.description;
        upgradeEffect.text = "Upgrade: " + defDetail.upgrade;

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(() => menu.UpgradeDef());
    }

    private void ShowCRDetail()
    {
        SetActive();
        imageIcon.sprite = crDetail.image;
        title.text = crDetail.title;
        description.text = crDetail.description;
        upgradeEffect.text = "Upgrade: " + crDetail.upgrade;

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(() => menu.UpgradeCR());
    }

    private void ShowCDDetail()
    {
        SetActive();
        imageIcon.sprite = cdDetail.image;
        title.text = cdDetail.title;
        description.text = cdDetail.description;
        upgradeEffect.text = "Upgrade: " + cdDetail.upgrade;

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(() => menu.UpgradeCD());
    }

    private void ShowHPStats(int hp)
    {
        currentStats.text = $"Current: {hp} hp";
    }

    private void ShowAtkStats(int atk)
    {
        currentStats.text = $"Current: {atk} atk";
    }

    private void ShowDefStats(int def)
    {
        currentStats.text = $"Current: {def} def";
    }

    private void ShowCRStats(int rate)
    {
        currentStats.text = $"Current: {rate}% rate";
    }

    private void ShowCDStats(int dmg)
    {
        currentStats.text = $"Current: {dmg}% dmg";
    }

    private void ShowPrice(int price)
    {
        priceText.text = $"{price} to upgrade";
    }
}

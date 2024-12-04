using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button ContinueButton;
    public PlayerStatsSO playerStats;

    public GameObject Alert;

    public AudioClip sceneBGM;

    // Start is called before the first frame update
    void Start()
    {
        if (sceneBGM != null)
        {
            BGMManager.Instance.PlayBGM(sceneBGM);
            BGMManager.Instance.FadeInBGM();
        }

        if (SaveSystem.SaveFileExists())
        {
            ContinueButton.interactable = true;
            playerStats.IsSaved = true;
        }
        else
        {
            ContinueButton.interactable = false;
            playerStats.IsSaved = false;
        }
    }

    private IEnumerator TransitionToScene(int buildidx)
    {
        BGMManager.Instance.FadeOutBGM();

        yield return new WaitForSeconds(BGMManager.Instance.fadeDuration);
        SaveSystem.LoadPlayerStats(playerStats);
        Debug.Log("Game data loaded!");

        SceneManager.LoadScene(buildidx);
    }

    public void LoadScene(int buildidx)
    {
        StartCoroutine(TransitionToScene(buildidx));
    }

    public void UpgradeMenu()
    {
       LoadScene(1);
    }

    public void PlayNewGame()
    {
        SaveSystem.DeleteSave();
        playerStats.ResetStats();
        LoadScene(1);
    }

    public void ShowAlert()
    {
        Alert.SetActive(true);
    }

    public void HideAlert()
    {
        Alert.SetActive(false);
    }

    public void NewGame()
    {
        if (playerStats.IsSaved)
        {
            ShowAlert();
        }else UpgradeMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

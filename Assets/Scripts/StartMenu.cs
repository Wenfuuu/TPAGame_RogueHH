using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button ContinueButton;
    public PlayerStatsSO player;

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

        if (player.IsSaved)
        {
            ContinueButton.interactable = true;
        }
        else
        {
            ContinueButton.interactable = false;
        }
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
        if (player.IsSaved)
        {
            ShowAlert();
        }else PlayGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

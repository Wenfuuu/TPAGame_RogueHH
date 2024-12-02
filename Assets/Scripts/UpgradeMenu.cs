using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeMenu : MonoBehaviour
{
    public AudioClip sceneBGM;
    public PlayerStatsSO playerStats;
    public IntEventChannel UpdateZhen;

    private void Start()
    {
        if (sceneBGM != null)
        {
            BGMManager.Instance.PlayBGM(sceneBGM);
            BGMManager.Instance.FadeInBGM();
        }

        //raise event for UI (anggap script ini kyk player di game)
        UpdateZhen.RaiseEvent(playerStats.Zhen);
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

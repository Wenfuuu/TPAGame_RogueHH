using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePrefab;

    private bool IsActive = false;

    public AudioClip sceneBGM;
    public AudioClip battleBGM;

    private void Start()
    {
        if (sceneBGM != null)
        {
            BGMManager.Instance.PlayBGM(sceneBGM);
            BGMManager.Instance.FadeInBGM();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("toggling pause");
            TogglePause();
        }

        if (GameManager.Instance.CheckBattle())
        {
            StartCoroutine(TransitionBGM(battleBGM));
        }else StartCoroutine(TransitionBGM(sceneBGM));
    }

    public void TogglePause()
    {
        if(IsActive)
        {
            HidePause();
        }
        else
        {
            ShowPause();
        }
    }

    public void ShowPause()
    {
        //kasi sfx open inventory
        SFXManager.Instance.PlaySFX(Sounds.Instance.InventoryOpenSFX, transform, 1f);
        IsActive = true;
        PausePrefab.SetActive(true);
        LockPlayerInput(true);
    }

    public void HidePause()
    {
        //kasi sfx close inventory
        SFXManager.Instance.PlaySFX(Sounds.Instance.InventoryCloseSFX, transform, 1f);
        IsActive = false;
        PausePrefab.SetActive(false);
        LockPlayerInput(false);
    }

    private void LockPlayerInput(bool locked)
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<PlayerStateMachine>();
            if (controller != null)
            {
                controller.enabled = !locked;
            }
        }
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

    public void BackToUpgrade()
    {
        LoadScene(1);
    }

    public void ExitToMenu()
    {
        SaveSystem.SavePlayerStats(PlayerStateMachine.Instance.gameObject.GetComponent<PlayerDamageable>().playerStats);
        LoadScene(0);
    }

    public void NextFloor()
    {
        PlayerStateMachine player = PlayerStateMachine.Instance;
        if(player.GetComponent<PlayerDamageable>().playerStats.CurrentFloor == 1)
        {
            BackToUpgrade();
        }
        else LoadScene(2);
    }
}

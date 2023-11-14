using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    public SettingsScript settingsScript;

    public AudioMixer mainMixer;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);

        mainMixer.SetFloat("mainVolume", Mathf.Log10(PlayerPrefs.GetFloat("mainVolume", 1f)) * 20);
        mainMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume", 0.5f)) * 20);
        mainMixer.SetFloat("sfxVolume", Mathf.Log10(PlayerPrefs.GetFloat("sfxVolume", 1f)) * 20);

        if (!ProfileManager.IsInitalized)
        {
            playButton.interactable = false;
            ProfileManager.Instance.onProfileManagerInitalized.AddListener(() =>
            {
                playButton.interactable = true;
            });
        }
    }

    public void PlayGame()
    {
        if (!ProfileManager.Instance.IsSignedIn)
            return;

        SceneManager.LoadScene("Main Game");
    }

    public void OpenSettings()
    {
        HideMenu();
        settingsScript.ShowMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }
}

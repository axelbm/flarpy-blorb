using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public MainMenuScript mainMenuScript;

    public AudioMixer mainMixer;

    public Slider mainSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        mainSlider.onValueChanged.AddListener(SetMainVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        mainSlider.value = PlayerPrefs.GetFloat("mainVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);

        backButton.onClick.AddListener(BackToMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainVolume(float volume)
    {
        PlayerPrefs.SetFloat("mainVolume", volume);

        if (volume == 0)
            mainMixer.SetFloat("mainVolume", -80f);
        else
            mainMixer.SetFloat("mainVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);

        if (volume == 0)
            mainMixer.SetFloat("musicVolume", -80f);
        else
            mainMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);

        if (volume == 0)
            mainMixer.SetFloat("sfxVolume", -80f);
        else
            mainMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
    }

    public void BackToMenu()
    {
        HideMenu();
        mainMenuScript.ShowMenu();
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

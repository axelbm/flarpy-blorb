using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore;

    public ControllerScript controllerScript;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI controlsHelpText;
    
    private bool gameRunning = true;
    private bool isSleeping = false;
    private bool isInitalSleep = true;
    private bool gameIsOver = false;
    private float gameOverAtTime;
    public float timeBeforeRestart = 0.5f;

    public AudioSource gameOverSound;
    public AudioSource gameOverSoundHighScore;
    public AudioSource startMusic;
    public AudioSource gameMusic;
    public AudioSource initialSleepMusic;

    public ParticleSystem backgroundParticles;

    public float musicVolume = 0.5f;
    public float soundEffectVolume = 0.5f;

    private float currentMusicVolume;
    private float targetMusicVolume;


    public void Start()
    {
        backgroundParticles.Play();
        // backgroundParticles.

        initialSleepMusic.Play();

        gameOverSound.volume = soundEffectVolume;
        gameOverSoundHighScore.volume = soundEffectVolume;

        controlsHelpText.text = "Press [Space] to flap";
        controlsHelpText.gameObject.SetActive(true);

        targetMusicVolume = musicVolume * 0.2f;
        currentMusicVolume = targetMusicVolume;

        isInitalSleep = true;

        SleepGame();
    }

    public void AwakeGame()
    {
        if (isInitalSleep == true)
        {
            InitalAwakeGame();
            return;
        }

        Time.timeScale = 1;
        isSleeping = false;

        targetMusicVolume = musicVolume;

        controllerScript.Mode = "game";
    }

    private void InitalAwakeGame()
    {
        Debug.Log("Inital Awake Game");

        controlsHelpText.gameObject.SetActive(false);

        targetMusicVolume = musicVolume;

        startMusic.Play();
        gameMusic.PlayDelayed(startMusic.clip.length - 0.05f);

        isInitalSleep = false;

        AwakeGame();
    }

    public void SleepGame()
    {
        Time.timeScale = 0;
        isSleeping = true;

        targetMusicVolume = musicVolume * 0.5f;
        
        controllerScript.Mode = "sleep";
    }


    [ContextMenu("Increase Score")]
    public void AddScore(int scoreIncrease = 1)
    {
        playerScore = playerScore + scoreIncrease;
        scoreText.text = playerScore.ToString();
    }

    [ContextMenu("Pause Game")]
    public void PauseGame()
    {
        Time.timeScale = 0;
        gameRunning = false;

        targetMusicVolume = musicVolume * 0.5f;

        controllerScript.Mode = "pause";
    }

    [ContextMenu("Resume Game")]
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameRunning = true;

        targetMusicVolume = musicVolume;

        controllerScript.Mode = "game";
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }

    public void GameOver()
    {
        gameIsOver = true;
        gameOverAtTime = Time.realtimeSinceStartup;
        PauseGame();

        if (playerScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
            gameOverSoundHighScore.Play();
        }
        else
        {
            gameOverSound.Play();
        }

        gameOverScreen.SetActive(true);

        controllerScript.Mode = "gameOver";
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Update()
    {
        currentMusicVolume = Mathf.Lerp(currentMusicVolume, targetMusicVolume, Time.unscaledDeltaTime * 5);

        startMusic.volume = currentMusicVolume;
        gameMusic.volume = currentMusicVolume;
        initialSleepMusic.volume = currentMusicVolume;
    }
}

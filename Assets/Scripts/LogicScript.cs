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

    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    
    private bool gameRunning = true;
    private bool gameIsOver = false;
    private float gameOverAtTime;
    private float timeBeforeRestart = 0.5f;

    public AudioSource gameOverSound;
    public AudioSource gameOverSoundHighScore;
    public AudioSource startMusic;
    public AudioSource gameMusic;

    public float musicVolume = 0.5f;
    public float soundEffectVolume = 0.5f;

    private float currentMusicVolume;
    private float targetMusicVolume;


    public void Start()
    {
        currentMusicVolume = musicVolume;
        targetMusicVolume = musicVolume;

        startMusic.Play();
        gameMusic.PlayDelayed(startMusic.clip.length - 0.05f);

        gameOverSound.volume = soundEffectVolume;
        gameOverSoundHighScore.volume = soundEffectVolume;
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
    }

    [ContextMenu("Resume Game")]
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameRunning = true;

        targetMusicVolume = musicVolume;
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

        if (gameIsOver == true)
        {
            if (Time.realtimeSinceStartup > gameOverAtTime + timeBeforeRestart)
            {   
                if (Input.GetKeyDown(KeyCode.Space) == true)
                    RestartGame();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                if (gameRunning == true)
                    PauseGame();
                else
                    ResumeGame();
            }
        }
    }
}

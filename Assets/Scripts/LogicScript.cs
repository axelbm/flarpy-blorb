using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore;

    public ControllerScript controllerScript;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public TextMeshProUGUI profileNameText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI controlsHelpText;


    private bool gameRunning = true;
    private bool isSleeping = false;
    private bool isInitalSleep = true;
    private bool gameIsOver = false;
    private float gameOverAtTime;
    private bool canRestart = true;
    public float timeBeforeRestart = 0.5f;

    public AudioSource gameOverSound;
    public AudioSource gameOverSoundHighScore;
    public AudioSource startMusic;
    public AudioSource gameMusic;
    public AudioSource initialSleepMusic;

    public ParticleSystem backgroundParticles;

    public LeaderBoard leaderBoard;

    public float musicVolume = 1f;
    public float soundEffectVolume = 1f;

    private float currentMusicVolume;
    private float targetMusicVolume;

    public static LeaderboardEntry lastHighScore;
    public static LogicScript Instance;

    public LogicScript()
    {
        Instance = this;
    }


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

        profileNameText.text = ProfileManager.Instance.PlayerName;
        scoreText.text = 0.ToString();
        highScoreText.text = ProfileManager.Instance.HighScore.Score.ToString();
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
    public void PauseGame(bool showPauseMenu = true)
    {
        Time.timeScale = 0;
        gameRunning = false;

        targetMusicVolume = musicVolume * 0.5f;

        controllerScript.Mode = "pause";

        if (showPauseMenu)
            pauseScreen.SetActive(true);
    }

    [ContextMenu("Resume Game")]
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameRunning = true;

        targetMusicVolume = musicVolume;

        controllerScript.Mode = "game";

        pauseScreen.SetActive(false);
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }

    public async void GameOver()
    {
        canRestart = false;
        gameIsOver = true;
        gameOverAtTime = Time.realtimeSinceStartup;
        PauseGame(false);


        if (playerScore > ProfileManager.Instance.HighScore.Score)
        {
            gameOverSoundHighScore.Play();
        }
        else
        {
            gameOverSound.Play();
        }

        gameOverScreen.SetActive(true);

        controllerScript.Mode = "gameOver";
        
        await ProfileManager.Instance.AddScore(playerScore);

        canRestart = true;
    }

    public async void SwitchProfile(string profileName)
    {
        await leaderBoard.UpdatePlayerName(profileName);
        lastHighScore = await leaderBoard.GetPlayerScore();
    }

    public void RestartGame()
    {
        if (canRestart == false)
            return;

        Time.timeScale = 1;
        SceneManager.LoadScene("Main Game");
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    void Update()
    {
        currentMusicVolume = Mathf.Lerp(currentMusicVolume, targetMusicVolume, Time.unscaledDeltaTime * 5);

        startMusic.volume = currentMusicVolume;
        gameMusic.volume = currentMusicVolume;
        initialSleepMusic.volume = currentMusicVolume;
    }
}

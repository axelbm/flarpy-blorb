using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Button playButton;

    // Start is called before the first frame update
    async void Start()
    {
        playButton.interactable = false;

        await ProfileManager.Initalize();

        LeaderboardEntry highScore = ProfileManager.Instance.HighScore;

        Debug.Log(JsonConvert.SerializeObject(highScore));

        playButton.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

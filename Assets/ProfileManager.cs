using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.Events;

public class ProfileManager : MonoBehaviour
{
    public class ProfileSwitchedEvent : UnityEvent<string> { }
    public class ProfileManegerInitalizedEvent : UnityEvent { }

    public static ProfileManager Instance { get; private set; }

    private static bool isInitalized = false;
    public static bool IsInitalized
    {
        get
        {
            return isInitalized;
        }
    }

    public static string PlayerProfile
    {
        get
        {
            return PlayerPrefs.GetString("PlayerProfile", "Player");
        }
    }


    public bool IsSignedIn
    {
        get
        {
            return isInitalized && AuthenticationService.Instance.IsSignedIn;
        }
    }

    public string PlayerId
    {
        get
        {
            return AuthenticationService.Instance.PlayerId;
        }
    }

    public string PlayerName
    {
        get
        {
            if (AuthenticationService.Instance.PlayerName == null)
                return PlayerProfile;

            return AuthenticationService.Instance.PlayerName.Split('#')[0];
        }
    }

    public string PlayerNameId
    {
        get
        {
            return AuthenticationService.Instance.PlayerName.Split('#')[1];
        }
    }

    public PlayerInfo PlayerInfo
    {
        get
        {
            return AuthenticationService.Instance.PlayerInfo;
        }
    }

    public ProfileSwitchedEvent onProfileSwitched = new();
    public ProfileManegerInitalizedEvent onProfileManagerInitalized = new();

    private readonly string leaderboardId = "flarpy-blorb";
    private static LeaderboardEntry highScore;
    public LeaderboardEntry HighScore
    {
        get
        {
            return highScore;
        }
    }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public async static Task Initalize()
    {
        await UnityServices.InitializeAsync();
        isInitalized = true;

        List<string> profiles = GetProfiles();
        string profileName = PlayerPrefs.GetString("PlayerProfile", "Player");

        if (profiles.Count == 0)
        {
            AddProfile(profileName);
        }

        if (!AssertProfile(profileName))
        {
            AddProfile(profileName);
        }

        await SwitchProfile(profileName);

        Instance.onProfileManagerInitalized.Invoke();
    }


    public static List<string> GetProfiles()
    {
        int profileCount = PlayerPrefs.GetInt("ProfileCount", 0);
        List<string> profiles = new List<string>();

        for (int i = 0; i < profileCount; i++)
        {
            profiles.Add(PlayerPrefs.GetString("Profile#" + i, "Player"));
        }

        return profiles;
    }

    public static bool AssertProfile(string profileName)
    {
        List<string> profiles = GetProfiles();

        foreach (string profile in profiles)
        {
            if (profile == profileName)
            {
                return true;
            }
        }

        return false;
    }

    public static void AddProfile(string profileName)
    {
        List<string> profiles = GetProfiles();

        if (!AssertProfile(profileName))
        {
            profiles.Add(profileName);
            PlayerPrefs.SetInt("ProfileCount", profiles.Count);
            PlayerPrefs.SetString("Profile#" + (profiles.Count - 1), profileName);
        }
    }

    public static void RemoveProfile(string profileName)
    {
        List<string> profiles = GetProfiles();

        if (AssertProfile(profileName))
        {
            profiles.Remove(profileName);
            PlayerPrefs.SetInt("ProfileCount", profiles.Count);

            for (int i = 0; i < profiles.Count; i++)
            {
                PlayerPrefs.SetString("Profile#" + i, profiles[i]);
            }
        }
    }

    public static void RenameProfile(string oldProfileName, string newProfileName)
    {
        List<string> profiles = GetProfiles();

        if (AssertProfile(oldProfileName))
        {
            profiles[profiles.IndexOf(oldProfileName)] = newProfileName;
            PlayerPrefs.SetInt("ProfileCount", profiles.Count);

            for (int i = 0; i < profiles.Count; i++)
            {
                PlayerPrefs.SetString("Profile#" + i, profiles[i]);
            }
        }
    }

    public async static Task SwitchProfile(string profileName)
    {
        if (!AssertProfile(profileName))
            throw new Exception("Profile does not exist.");
        if (!isInitalized)
            throw new Exception("ProfileManager is not initalized.");

        Debug.Log("IsInitalized: " + isInitalized + " IsSignedIn: " + AuthenticationService.Instance.IsSignedIn);
        if (Instance.IsSignedIn)
            Instance.SignOut();

        PlayerPrefs.SetString("PlayerProfile", profileName);

        AuthenticationService.Instance.SwitchProfile(profileName);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        if (Instance.PlayerName != profileName)
        {
            await Instance.RenamePlayer(profileName);

            // await Instance.AddScore(0);
        }
        await Instance.AddScore(0);
        await Instance.LoadHighScore();

        Instance.onProfileSwitched.Invoke(profileName);
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
    }

    public async Task RenamePlayer(string newProfileName)
    {
        if (!isInitalized)
            throw new Exception("ProfileManager is not initalized.");

        if (!Instance.IsSignedIn)
            throw new Exception("ProfileManager is not signed in.");

        await AuthenticationService.Instance.UpdatePlayerNameAsync(newProfileName);

        Debug.Log("Signed in as: " + PlayerId);
        Debug.Log("PlayerName: " + PlayerName);
        Debug.Log("PlayerInfo: " + JsonConvert.SerializeObject(PlayerInfo));
    }

    public async Task<LeaderboardEntry> LoadHighScore()
    {
        if (!isInitalized)
            throw new Exception("ProfileManager is not initalized.");

        if (!Instance.IsSignedIn)
            throw new Exception("ProfileManager is not signed in.");

        highScore =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);

        return highScore;
    }

    public async Task<LeaderboardEntry> AddScore(int score)
    {
        if (!isInitalized)
            throw new Exception("ProfileManager is not initalized.");

        if (!Instance.IsSignedIn)
            throw new Exception("ProfileManager is not signed in.");

        highScore = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);

        return highScore;
    }
}

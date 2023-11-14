using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManagerUI : MonoBehaviour
{
    public GameObject diplayProfilePanel;
    public TMP_Dropdown profilesDropdown;


    public GameObject controlePanel;
    public Button newProfileButton;
    public Button deleteProfileButton;
    public Button renameProfileButton;


    public GameObject newProfilePanel;
    public TMP_InputField newProfileInputField;
    public Button newProfileConfirmButton;
    public Button newProfileCancelButton;


    public GameObject renameProfilePanel;
    public TMP_InputField renameProfileInputField;
    public Button renameProfileConfirmButton;
    public Button renameProfileCancelButton;

    void Start()
    {
        UpadteDisplayProfilePanel();

        profilesDropdown.onValueChanged.AddListener((value) => { SwitchProfile(); });

        newProfileButton.onClick.AddListener(ShowNewProfilePanel);
        newProfileConfirmButton.onClick.AddListener(AddNewProfile);
        newProfileCancelButton.onClick.AddListener(HideNewProfilePanel);

        renameProfileButton.onClick.AddListener(ShowRenameProfilePanel);
        // renameProfileConfirmButton.onClick.AddListener(RenameProfile);
        renameProfileCancelButton.onClick.AddListener(HideRenameProfilePanel);

        // ProfileManager.Instance.onProfileSwitched.AddListener(UpadteDisplayProfilePanel);
    }

    private void UpadteDisplayProfilePanel(string profileName = null)
    {
        List<string> profiles = ProfileManager.GetProfiles();

        profilesDropdown.ClearOptions();
        profilesDropdown.AddOptions(profiles);
        profilesDropdown.value = profiles.IndexOf(ProfileManager.PlayerProfile);
        
        deleteProfileButton.interactable = profiles.Count > 1;
    }

    public async void SwitchProfile()
    {
        await ProfileManager.SwitchProfile(profilesDropdown.options[profilesDropdown.value].text);
    }

    // ### New Profile ###
    public void ShowNewProfilePanel()
    {
        newProfilePanel.SetActive(true);
        newProfileInputField.text = "";

        diplayProfilePanel.SetActive(false);
    }

    public void HideNewProfilePanel()
    {
        newProfilePanel.SetActive(false);

        diplayProfilePanel.SetActive(true);
    }

    public async void AddNewProfile()
    {
        string profileName = newProfileInputField.text;

        if (profileName != "")
        {
            ProfileManager.AddProfile(profileName);
            await ProfileManager.SwitchProfile(profileName);
            UpadteDisplayProfilePanel();

            HideNewProfilePanel();
        }
    }


    // ### Rename Profile ###
    public void ShowRenameProfilePanel()
    {
        renameProfilePanel.SetActive(true);
        renameProfileInputField.text = ProfileManager.PlayerProfile;

        diplayProfilePanel.SetActive(false);
    }

    public void HideRenameProfilePanel()
    {
        renameProfilePanel.SetActive(false);

        diplayProfilePanel.SetActive(true);
    }
}

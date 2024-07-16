using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField]
    GameObject Settings;

    [SerializeField]
    GameObject continueButton;
    bool settingsOpen;
    private void Start()
    {
        //WwisePlay MuMainMenuTheme
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (PlayerPrefs.GetInt("LevelIndex") == 0)
        {
            Destroy(continueButton);
        }
        DefaultPreferences();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsOpen)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
    }

    private void DefaultPreferences()
    {
        if (Settings.TryGetComponent<SettingsManager>(out SettingsManager sm))
        {
            sm.DefaultPreferences();
        }
    }

    public void StartGame()
    {
        //WwisePlay UIButtonPress
        //WwisePlay UIContinueGame
        Debug.Log("LoadingTheGame");
        SceneManager.LoadScene("LevelScene");
    }

    public void NewGame()
    {
        //WwisePlay UIButtonPress
        //WwisePlay UIStartNewGame
        Debug.Log("LoadingTheGame");
        PlayerPrefs.SetInt("LevelIndex", 0);
        PlayerPrefs.SetInt("StarCount", 0);
        PlayerPrefs.SetFloat("Timer", 0);
        SceneManager.LoadScene("StartGameScene");
    }

    public void QuitGame()
    {
        //WwisePlay UIButtonPress
        Application.Quit();
    }

    public void OpenSettings()
    {
        //WwisePlay UIButtonPress
        //WwisePlay UIOpenSettingsJingle
        settingsOpen = true;
        Settings.SetActive(true);
        if (Settings.TryGetComponent<SettingsManager>(out SettingsManager sm))
        {
            sm.OpenSettings();
        }
    }

    public void CloseSettings()
    {
        //WwisePlay UICloseSettingsJingle
        //WwisePlay UIButtonPress
        settingsOpen = false;
        Settings.SetActive(false);
    }

    public void OpenCredits()
    {
        //WwisePlay UIOpenCreditsJingle
        //WwisePlay UIButtonPress
        SceneManager.LoadScene("Credits");
    }

    public void GoToMainMenu()
    {
        //WwisePlay UIButtonPress
        SceneManager.LoadScene("MainMenu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float MouseSensitivity;
    [SerializeField]
    GameObject gameMenu, settingsMenu;
    bool openMenu;
    //[SerializeField]
    //Slider slider;
    [SerializeField]
    float baseMouseSpeed;

    [SerializeField]
    GameObject[] Bushes;

    [SerializeField]
    GameObject[] Levels;
    public int levelIndex;
    GameObject currentLevel;
    [SerializeField]
    Image ScreenWiper;
    [SerializeField]
    float screenWipeSpeed;
    [SerializeField]
    float screenWipeSeconds;
    [SerializeField]
    bool testingLevels;
    [SerializeField]
    int loadLevelViaIndex;

    [SerializeField]
    private TextMeshProUGUI starText;
    public int StarAmount;
    [SerializeField]
    private TextMeshProUGUI timerUI;
    private float timer;
    [SerializeField]
    //private Slider masterSlider, effectsSlider, musicSlider;
    //[SerializeField]
    SettingsManager settings;
    //[SerializeField]


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSpeed", 0.5f * baseMouseSpeed);
    }

    private void Start()
    {
        if (testingLevels)
        {
            LoadLevelIndex(loadLevelViaIndex);
            PlayerPrefs.SetInt("LevelIndex", 0);
            PlayerPrefs.SetInt("StarAmount", 0);
            PlayerPrefs.SetFloat("Timer", 0);
        }
        else
        {
            LoadLevelIndex(PlayerPrefs.GetInt("LevelIndex"));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!openMenu)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
        timer += Time.deltaTime;
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        //timerUI.text = String.Format("{00.00}", timer);
        //timerUI.text = $"{minutes}:{seconds}";
        timerUI.text = ((minutes < 10) ? ("0") : ("")) + minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString();
        //timerUI.text = TimeSpan.FromSeconds((double)timer).ToString("mm:ss");
    }

    public void LockInput(bool lockInput)
    {
        GloopMain.Instance.LockInput(lockInput);
    }

    //public void ChangeMouseSpeed()
    //{
    //    MouseSensitivity = slider.value * baseMouseSpeed;
    //    if (GloopMain.Instance != null)
    //    {
    //        GloopMain.Instance.lookSensitivity = MouseSensitivity;
    //    }
    //    PlayerPrefs.SetFloat("MouseSpeed", MouseSensitivity);
    //}

    public void LoadLevelIndex(int index)
    {
        if (index >= Levels.Length)
        {
            index = 0;
        }
        if (index == 0)
        {
            StarAmount = 0;
        }
        else
        {
            SetStar(PlayerPrefs.GetInt("StarAmount"));
            timer = PlayerPrefs.GetFloat("Timer");

        }
        Destroy(currentLevel);
        levelIndex = index;
        StartCoroutine(StartGame(index));
    }

    public void NextLevel()
    {
        levelIndex++;
        //////////////////////////////////SAVE OBJECTS WITH PLAYER PREFS AND LEVEL NUMBER WITH PLAYER PREFS
        PlayerPrefs.SetInt("LevelIndex", levelIndex);
        PlayerPrefs.SetInt("StarAmount", StarAmount);
        PlayerPrefs.SetFloat("Timer", timer);
        GloopMain.Instance.SaveProgress();
        StartCoroutine(LoadLevel(levelIndex));
    }

    private IEnumerator StartGame(int index)
    {
        Vector4 imageAlpha = ScreenWiper.color;
        currentLevel = Instantiate(Levels[index]);
        for (float i = screenWipeSeconds; i > 0; i -= Time.deltaTime * screenWipeSpeed)
        {
            imageAlpha.w = i / screenWipeSeconds;
            ScreenWiper.color = imageAlpha;
            yield return new WaitForSeconds(0);
        }
        imageAlpha.w = 0;
        ScreenWiper.color = imageAlpha;
    }

    private IEnumerator LoadLevel(int index)
    {
        Vector4 imageAlpha = ScreenWiper.color;
        //levelIndex++;
        for (float i = 0; i < screenWipeSeconds; i += Time.deltaTime * screenWipeSpeed)
        {
            yield return new WaitForSeconds(0);
            imageAlpha.w = i / screenWipeSeconds;
            ScreenWiper.color = imageAlpha;
        }
        if (levelIndex < Levels.Length)
        {
            Destroy(currentLevel);
            currentLevel = Instantiate(Levels[index]);
        }
        else
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            PlayerPrefs.SetInt("LevelIndex", 0);
            PlayerPrefs.SetInt("StarAmount", StarAmount);
            PlayerPrefs.SetFloat("Timer", timer);
            SceneManager.LoadScene("EndScreen");
        }
        for (float i = screenWipeSeconds; i > 0; i -= Time.deltaTime * screenWipeSpeed)
        {
            yield return new WaitForSeconds(0);
            imageAlpha.w = i / screenWipeSeconds;
            ScreenWiper.color = imageAlpha;
        }
        imageAlpha.w = 0;
        ScreenWiper.color = imageAlpha;
    }

    public void SetStar(int amount)
    {
        StarAmount = amount;
        starText.text = $"{StarAmount}";
    }

    public void AddStar()
    {
        StarAmount++;
        starText.text = $"{StarAmount}";
    }

    public void RemoveStar()
    {
        StarAmount--;
        starText.text = $"{StarAmount}";
    }

    public void ToMainMenu()
    {
        CloseMenu();
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        openMenu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        LockInput(true);
        gameMenu.SetActive(true);
        settings.OpenSettings();
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        openMenu = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        LockInput(false);
        gameMenu.SetActive(false);
        settingsMenu.SetActive(false);
        openMenu = false;
    }

    public void OpenSettings()
    {
        //slider.value = MouseSensitivity / baseMouseSpeed;
        //masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        //effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        //musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        gameMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        openMenu = true;
        gameMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void DisableBushes()
    {
        for (int i = 0; i < Bushes.Length; i++)
        {
            Bushes[i].SetActive(false);
        }
    }

    //public void ChangeSound(int index)
    //{
    //    switch (index)
    //    {
    //        case 0:
    //            SoundManager.Instance.ChangeMasterVolume(masterSlider.value);
    //            break;
    //        case 1:
    //            SoundManager.Instance.ChangeSoundEffectVolume(effectsSlider.value);
    //            break;
    //        case 2:
    //            SoundManager.Instance.ChangeMusicVolume(musicSlider.value);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void CloseGame()
    {
        Application.Quit();
    }
}

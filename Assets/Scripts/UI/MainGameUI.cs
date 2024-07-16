using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainGameUI : MonoBehaviour
{
    public static MainGameUI Instance;
    [SerializeField]
    GameManager gm;

    [SerializeField]
    GameObject gameMenu, settingsMenu;

    [SerializeField]
    GameObject[] Bushes;
    [SerializeField]
    Image ScreenWiper;
    public float ScreenWipeSpeed;
    public float ScreenWipeSeconds;
    [SerializeField]
    SettingsManager settings;
    bool openMenu;

    [SerializeField]
    private TextMeshProUGUI starText;
    [SerializeField]
    private TextMeshProUGUI timerUI;

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
    }

    private void Start()
    {
        GameManager.Instance.ChangeStarAmount.AddListener(UpdateStarAmount);
    }

    private void Update()
    {
        int minutes = (int)gm.Timer / 60;
        int seconds = (int)gm.Timer % 60;
        timerUI.text = ((minutes < 10) ? ("0") : ("")) + minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString();
    }

    public void PressEscaoe(InputAction.CallbackContext context)
    {
        if (context.started)
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
    }


    public IEnumerator WipeScreen()
    {
        Vector4 imageAlpha = ScreenWiper.color;
        for (float i = 0; i < ScreenWipeSeconds; i += Time.deltaTime * ScreenWipeSpeed)
        {
            yield return new WaitForSeconds(0);
            imageAlpha.w = i / ScreenWipeSeconds;
            ScreenWiper.color = imageAlpha;
        }
    }

    public IEnumerator UnwipeScreen()
    {
        Vector4 imageAlpha = ScreenWiper.color;
        for (float i = ScreenWipeSeconds; i > 0; i -= Time.deltaTime * ScreenWipeSpeed)
        {
            yield return new WaitForSeconds(0);
            imageAlpha.w = i / ScreenWipeSeconds;
            ScreenWiper.color = imageAlpha;
        }
        imageAlpha.w = 0;
        ScreenWiper.color = imageAlpha;
    }

    public void ToMainMenu()
    {
        CloseMenu();
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateStarAmount()
    {
        starText.text = gm.StarAmount.ToString();
    }

    public void OpenMenu()
    {
        //WwisePlay UIMenuOpenJingle
        gm.TimeScalers++;
        gm.CursorUnlockers++;
        openMenu = true;
        gameMenu.SetActive(true);
        settings.OpenSettings();
    }

    public void CloseMenu()
    {
        //WwisePlay UIMenuCloseJingle
        gm.TimeScalers--;
        gm.CursorUnlockers--;
        openMenu = false;
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
        //WwisePlay UIOpenSettingsJingle
        gameMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        //WwisePlay UICloseSettingsJingle
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
}

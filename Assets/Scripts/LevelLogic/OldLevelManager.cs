using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OldLevelManager : MonoBehaviour
{
    public static OldLevelManager Instance;
    [SerializeField]
    bool hello;
    //[SerializeField]
    //MainGameUI ui;
    [SerializeField]
    GameManager gm;
    [SerializeField]
    GameObject[] Levels;
    public int levelIndex;
    GameObject currentLevel;
    [SerializeField]
    bool testingLevels;
    [SerializeField]
    int loadLevelViaIndex;

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

    public void LoadLevelIndex(int index)
    {
        if (index >= Levels.Length)
        {
            index = 0;
        }
        if (index == 0)
        {
            gm.StarAmount = 0;
        }
        else
        {
            gm.SetStar(PlayerPrefs.GetInt("StarAmount"));
            gm.Timer = PlayerPrefs.GetFloat("Timer");

        }
        Destroy(currentLevel);
        levelIndex = index;
        StartGame(index);
    }

    public void NextLevel()
    {
        levelIndex++;
        //////////////////////////////////SAVE OBJECTS WITH PLAYER PREFS AND LEVEL NUMBER WITH PLAYER PREFS
        PlayerPrefs.SetInt("LevelIndex", levelIndex);
        PlayerPrefs.SetInt("StarAmount", gm.StarAmount);
        PlayerPrefs.SetFloat("Timer", gm.Timer);

        if (levelIndex < Levels.Length)
        {
            Destroy(currentLevel);
            currentLevel = Instantiate(Levels[levelIndex]);
        }
        else
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            PlayerPrefs.SetInt("LevelIndex", 0);
            PlayerPrefs.SetInt("StarAmount", gm.StarAmount);
            PlayerPrefs.SetFloat("Timer", gm.Timer);
            SceneManager.LoadScene("EndScreen");
        }

        //GloopMain.Instance.SaveProgress();
        //StartCoroutine(LoadLevel(levelIndex));
    }

    private void StartGame(int index)
    {
        currentLevel = Instantiate(Levels[index]);
        //StartCoroutine(ui.UnwipeScreen());
    }

    //private IEnumerator LoadLevel(int index)
    //{
    //    StartCoroutine(ui.WipeScreen());
    //    yield return new WaitForSeconds(ui.ScreenWipeSeconds * ui.ScreenWipeSpeed);
    //    if (levelIndex < Levels.Length)
    //    {
    //        Destroy(currentLevel);
    //        currentLevel = Instantiate(Levels[index]);
    //    }
    //    else
    //    {
    //        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //        PlayerPrefs.SetInt("LevelIndex", 0);
    //        PlayerPrefs.SetInt("StarAmount", gm.StarAmount);
    //        PlayerPrefs.SetFloat("Timer", gm.Timer);
    //        SceneManager.LoadScene("EndScreen");
    //    }
    //    StartCoroutine(ui.UnwipeScreen());
    //}
}

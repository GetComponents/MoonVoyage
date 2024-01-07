using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreenLogic : MonoBehaviour
{
    [SerializeField]
    Vector2 randomPosX, randomPosY;
    [SerializeField]
    Image background;
    [SerializeField]
    Vector2 endingThreshhold;
    [SerializeField]
    Sprite bg0, bg1, bg2, bg3;
    [SerializeField]
    Image moonImage;
    [SerializeField]
    Sprite moon1, moon2, moon3;
    [SerializeField]
    Transform starParent;
    [SerializeField]
    GameObject starPrefab;
    [SerializeField]
    int starAmount;
    [SerializeField]
    bool testing;
    [SerializeField]
    float timeToSpawn;
    [SerializeField]
    TextMeshProUGUI timerUI, timerHighscoreUI;
    float timer;
    [SerializeField]
    AudioClip star1, star2, star3, star4, star5;
    [SerializeField]
    int starSoundeffectFrequency;
    float lastStarSound;

    private void Start()
    {
        if (testing)
        {

        }
        else
        {
            starAmount = PlayerPrefs.GetInt("StarAmount");
            timer = PlayerPrefs.GetFloat("Timer");
        }
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        timerUI.text = "Your time: " + ((minutes < 10) ? ("0") : ("")) + minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString();
        //timerUI.text = "Final Time: " + System.String.Format("{0:0.00}", timer);
        float highscore = PlayerPrefs.GetFloat("Highscore");

        if (highscore > timer || highscore < 1)
        {
            highscore = timer;
            PlayerPrefs.SetFloat("Highscore", timer);
            //timerHighscoreUI.text = "Your fastest time: " + timer;
        }
        minutes = (int)highscore / 60;
        seconds = (int)highscore % 60;
        timerHighscoreUI.text = "Your fastest time: " + ((minutes < 10) ? ("0") : ("")) + minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString();
        PlayerPrefs.SetFloat("Timer", 0);
        PlayerPrefs.SetInt("StarAmount", 0);
        StartCoroutine(SpawnStars());
        ChangeEndingScreen();
        //Screenwidth/StarAmount 
    }

    private void ChangeEndingScreen()
    {
        if (starAmount == 0)
        {
            background.sprite = bg0;
            moonImage.enabled = false;
        }
        else if (starAmount < endingThreshhold.x)
        {
            background.sprite = bg1;
            moonImage.sprite = moon1;
        }
        else if (starAmount < endingThreshhold.y)
        {
            background.sprite = bg2;
            moonImage.sprite = moon2;
        }
        else
        {
            background.sprite = bg3;
            moonImage.sprite = moon3;
        }

    }

    private void PlayRandomSound()
    {
        int tmp = Random.Range(0, 5);
        while (lastStarSound == tmp)
        {
            tmp = Random.Range(0, 5);
        }
        switch (tmp)
        {
            case 0:
                SoundManager.Instance.PlayEffect(star1);
                break;
            case 1:
                SoundManager.Instance.PlayEffect(star2);
                break;
            case 2:
                SoundManager.Instance.PlayEffect(star3);
                break;
            case 3:
                SoundManager.Instance.PlayEffect(star4);
                break;
            case 4:
                SoundManager.Instance.PlayEffect(star5);
                break;
            default:
                break;
        }
        lastStarSound = tmp;
    }

    private IEnumerator SpawnStars()
    {
        int starsSpawned = 0;
        for (float i = 0; i < starAmount; i += 1)
        {
            starsSpawned++;
            GameObject tmp = Instantiate(starPrefab, starParent);
            if (starsSpawned % starSoundeffectFrequency == 0)
            {
                PlayRandomSound();
            }
            tmp.GetComponent<RectTransform>().position = new Vector2(Mathf.Lerp(randomPosX.x, randomPosX.y, i / starAmount) * (Screen.width / 192), Random.Range(randomPosY.x, randomPosY.y) * (Screen.height / 108));
            yield return new WaitForSeconds(timeToSpawn / starAmount);
        }
    }
}

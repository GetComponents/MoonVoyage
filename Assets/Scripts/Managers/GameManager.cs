using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float MouseSensitivity;
    [SerializeField]
    float baseMouseSpeed;
    public int InputType;

    public int StarAmount;
    public float Timer;
    public UnityEvent PauseGame, UnpauseGame, ChangeStarAmount, LockInput, GravitySwitch;

    public int CursorUnlockers
    {
        get => m_cursorUnlockers;
        set
        {
            m_cursorUnlockers = value;
            LockInput?.Invoke();
            if (m_cursorUnlockers > 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    [SerializeField]
    private int m_cursorUnlockers;

    public int TimeScalers
    {
        get => m_timeScalers;
        set
        {
            m_timeScalers = value;
            if (m_timeScalers > 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    [SerializeField]
    private int m_timeScalers;


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


    private void Update()
    {
        Timer += Time.deltaTime;
    }

    public void SetStar(int amount)
    {
        StarAmount = amount;
        ChangeStarAmount.Invoke();
    }

    public void AddStar()
    {
        StarAmount++;
        ChangeStarAmount.Invoke();
    }

    public void RemoveStar()
    {
        StarAmount--;
        ChangeStarAmount.Invoke();

    }

    public void CloseGame()
    {
        Application.Quit();
    }
}

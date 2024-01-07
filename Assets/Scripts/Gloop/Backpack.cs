using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public static Backpack Instance;
    public int Coins;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("ALREADY BACKPACK INSTANCE IN SCENE");
            Destroy(gameObject);
        }
    }

    public void AddStar(int amount)
    {
        Coins += amount;
    }

    public void RemoveStar(int amount)
    {
        Coins += amount;
    }
}
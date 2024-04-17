using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Backpack : MonoBehaviour
{
    public static Backpack Instance;
    public int Coins;
    public List<GameObject> LosableObjects = new List<GameObject>();
    public UnityEvent Respawn;
    

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

    public void AddStar(GameObject star)
    {
        LosableObjects.Add(star);
        GameManager.Instance.AddStar();
    }

    public void RemoveStar(int amount)
    {
        Coins += amount;
    }

    public void RespawnPlayer()
    {
        foreach (GameObject gObject in LosableObjects)
        {
            if (gObject != null)
            {
                gObject.SetActive(true);
            }
        }
        foreach (GameObject gameObject in LosableObjects)
        {
            if (gameObject.tag == "Collectable")
            {
                GameManager.Instance.RemoveStar();
            }
        }
        LosableObjects = new List<GameObject>();
        Respawn?.Invoke();
    }

    public void SaveProgress()
    {
        int collectedStars = 0;
        foreach (GameObject gameObject in LosableObjects)
        {
            if (gameObject.tag == "Collectable")
            {
                collectedStars++;
            }
        }
        //Backpack.Instance.AddStar(collectedStars);
        //TODO: Actually save progress
        LosableObjects = new List<GameObject>();
    }
}
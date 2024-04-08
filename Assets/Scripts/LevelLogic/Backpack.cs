using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public static Backpack Instance;
    public int Coins;
    public Spawnpoint LastCheckpoint;
    public List<GameObject> LosableObjects = new List<GameObject>();
    

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

    private void Start()
    {
        GloopMain.Instance.Respawn.AddListener(RespawnPlayer);
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
        if (LastCheckpoint == null)
            return;
        foreach (GameObject gObject in LosableObjects)
        {
            if (gObject != null)
            {
                gObject.SetActive(true);
            }
        }
        foreach (GameObject gameObject in LosableObjects)
        {
            if (gameObject.TryGetComponent<ObjectProperty>(out ObjectProperty tmp) && tmp.Type == ObjectType.COLLECTABLE)
            {
                GameManager.Instance.RemoveStar();
            }
        }
        LosableObjects = new List<GameObject>();
        LastCheckpoint.RespawnPlayer();
    }

    public void SaveProgress()
    {
        int collectedStars = 0;
        foreach (GameObject gameObject in LosableObjects)
        {
            if (gameObject.TryGetComponent<ObjectProperty>(out ObjectProperty tmp) && tmp.Type == ObjectType.COLLECTABLE)
            {
                collectedStars++;
            }
        }
        //Backpack.Instance.AddStar(collectedStars);
        //TODO: Actually save progress
        LosableObjects = new List<GameObject>();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    [SerializeField]
    Spawnpoint firstCheckpoint;

    [SerializeField]
    bool deleteBushes;

    void Start()
    {
        GloopMain.Instance.LastCheckpoint = firstCheckpoint;
        GloopMain.Instance.RespawnPlayer();
        if (deleteBushes)
        {
            GameManager.Instance.DisableBushes();
        }
        //StartCoroutine(WaitForOneFrame());
    }



    //private IEnumerator WaitForOneFrame()
    //{
    //    yield return new WaitForSeconds(0);
    //}
}

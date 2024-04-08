using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    [SerializeField]
    Spawnpoint firstCheckpoint;

    void Start()
    {
        Backpack.Instance.LastCheckpoint = firstCheckpoint;
        GloopMain.Instance.RespawnPlayer();
        //StartCoroutine(WaitForOneFrame());
    }



    //private IEnumerator WaitForOneFrame()
    //{
    //    yield return new WaitForSeconds(0);
    //}
}

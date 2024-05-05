using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPlatforms : MonoBehaviour
{
    [SerializeField]
    Transform respawnTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Interacted with " + collision.gameObject.name);
        if (collision.tag == "PlayerFoot")
        {
            Backpack.Instance.Respawn.RemoveAllListeners();
            Backpack.Instance.Respawn.AddListener(RespawnPlayer);
            Backpack.Instance.SaveProgress();
        }
    }

    private void RespawnPlayer()
    {
        //GloopMain.Instance.CurrentMode = respawnMode;
        if (respawnTransform != null)
        {
            GloopMain.Instance.MyMovement.transform.position = respawnTransform.position;
        }
        else
        {
            Debug.LogWarning(gameObject.name + "(respawnPlatform does not have a respawnTransform attached!");
        }
    }
}

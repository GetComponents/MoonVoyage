using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPlatforms : MonoBehaviour
{
    [SerializeField]
    Transform respawnTransform;

    bool isTouchingFoot;
    bool interactedWithPlatform;

    [SerializeField]
    ObjectProperty op;

    private void Start()
    {
        op.InteractedWithPlayer.AddListener(() => { interactedWithPlatform = true; SavePlayerProgress(); });
        op.ExitInteractionWithPlayer.AddListener(() => { interactedWithPlatform = false; SavePlayerProgress(); });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Interacted with " + collision.gameObject.name);
        if (collision.tag == "PlayerFoot")
        {
            isTouchingFoot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerFoot")
        {
            isTouchingFoot = false;
        }
    }

    private void SavePlayerProgress()
    {
        if (isTouchingFoot && interactedWithPlatform)
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

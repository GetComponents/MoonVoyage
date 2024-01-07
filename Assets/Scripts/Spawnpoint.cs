using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField]
    EMode respawnMode;
    [SerializeField]
    CircleCollider2D myCollider;
    [SerializeField]
    SpriteRenderer sr;
    [SerializeField]
    GameObject particles;
    [SerializeField]
    AudioClip interactionSFX;


    public void RespawnPlayer()
    {
        GloopMain.Instance.CurrentMode = respawnMode;
        GloopMain.Instance.MyMovement.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && GloopMain.Instance.CurrentMode == respawnMode)
        {
            Debug.Log("Entered new Checkpoint");
            TriggerCheckpoint();
        }
    }

    private void TriggerCheckpoint()
    {
        SoundManager.Instance.PlayEffect(interactionSFX);
        gameObject.SetActive(false);
        Instantiate(particles, transform.position, Quaternion.identity);
        GloopMain.Instance.LastCheckpoint = this;
        GloopMain.Instance.SaveProgress();
        myCollider.enabled = false;
        sr.color = Color.black;
    }
}

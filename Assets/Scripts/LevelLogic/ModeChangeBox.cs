using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeChangeBox : MonoBehaviour
{
    [SerializeField]
    EMode myTransformation;
    [SerializeField]
    bool OneTimeUse;
    [SerializeField]
    AudioClip interactionSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (OneTimeUse)
            {
                Backpack.Instance.LosableObjects.Add(gameObject);
                gameObject.SetActive(false);
            }
            TransformPlayer();
            SoundManager.Instance.PlayEffect(interactionSound);
        }
    }

    private void TransformPlayer()
    {
        GloopMain.Instance.CurrentMode = myTransformation;
    }
}

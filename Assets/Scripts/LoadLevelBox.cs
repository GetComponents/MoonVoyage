using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.NextLevel();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}

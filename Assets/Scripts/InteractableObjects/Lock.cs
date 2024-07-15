using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField]
    GameObject parent;

    [SerializeField]
    float keyRequirement = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Backpack.Instance.KeyAmount >= keyRequirement)
        {
            for (int i = 0; i < keyRequirement; i++)
            {
                Backpack.Instance.RemoveKey();
            }
            Destroy(parent);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField]
    float deathTimer;
    void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

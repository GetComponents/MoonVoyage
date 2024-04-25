using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    [SerializeField]
    float timeTillDeath;
    float currentTime;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timeTillDeath)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleLauncher : MonoBehaviour
{
    [SerializeField]
    ObjectMover bubblePrefab;
    [SerializeField]
    float launchSpeed, launchFrequency;
    [SerializeField]
    Transform launchPos;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= launchFrequency)
        {
            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        ObjectMover tmp = Instantiate(bubblePrefab, launchPos.position, Quaternion.identity);
        //WwisePlay ObShootBubble
        tmp.MovementSpeed = launchSpeed;
        tmp.ProjectileDirection = transform.up.normalized;
        timer = 0;
    }
}

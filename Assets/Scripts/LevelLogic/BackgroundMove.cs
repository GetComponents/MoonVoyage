using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundMove : MonoBehaviour
{
    [SerializeField]
    GameObject leftScreen;
    [SerializeField]
    List<BackgroundObject> allObjects;
    [SerializeField]
    float paralaxSpeedX, paralaxSpeedY;
    float startPosX, startPosY;
    Vector3 currentPos;
    Vector2 spriteSize;
    [SerializeField]
    bool loopY;
    Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        currentPos.z = transform.position.z;
    }

    private void FixedUpdate()
    {
        MoveObject();
        LoopObject();
    }

    private void MoveObject()
    {
        currentPos.x = (cameraTransform.position.x * paralaxSpeedX) + startPosX;
        currentPos.y = (cameraTransform.position.y * paralaxSpeedY) + startPosY;
        transform.position = currentPos;
    }

    private void LoopObject()
    {
        float tmp = cameraTransform.position.x * (1 - paralaxSpeedX);
        float tmp2 = cameraTransform.position.y * (1 - paralaxSpeedY);

        if (tmp > spriteSize.x + startPosX)
        {
            startPosX += spriteSize.x;
        }
        else if (tmp < startPosX - spriteSize.x)
        {
            startPosX -= spriteSize.x;
        }
        if (!loopY)
            return;
        if (tmp2 > spriteSize.y + startPosY)
        {
            startPosY += spriteSize.y;
        }
        else if (tmp2 < startPosY - spriteSize.y)
        {
            startPosY -= spriteSize.y;
        }
    }
}

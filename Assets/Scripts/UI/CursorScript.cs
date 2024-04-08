using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    Camera mainCam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        mainCam = Camera.main;
    }
    void Update()
    {
        Cursor.visible = false;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos;
    }
}

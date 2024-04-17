using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounteractRotation : MonoBehaviour
{
    [SerializeField]
    ObjectProperty parentRot;
    Vector3 counterRot = new Vector3();
    private void Start()
    {
        counterRot.z = parentRot.RotationSpeed * -1;
        parentRot = null;
    }

    void Update()
    {
        transform.eulerAngles += counterRot * Time.deltaTime;
    }
}

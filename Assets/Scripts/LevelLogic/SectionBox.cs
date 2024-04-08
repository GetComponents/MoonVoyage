using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionBox : MonoBehaviour
{
    [SerializeField]
    SectionScript info;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Entered Box number " + info.SectionIndex);
            SectionLoader.Instance.LoadSection(info.SectionIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
    [SerializeField]
    float secondsForReactivation = 0.2f;

    [SerializeField]
    Collider2D col;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFoot"))
        {
            GloopMain.Instance.MyMovement.MyBase.OnJumpthroughPlatform++;
            GloopMain.Instance.MyMovement.MyBase.JumpthroughEvent.AddListener(Jumpthrough);
        }
        //Debug.Log(GloopMain.Instance.MyMovement.MyBase.OnJumpthroughPlatform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFoot"))
        {
            GloopMain.Instance.MyMovement.MyBase.OnJumpthroughPlatform--;
            GloopMain.Instance.MyMovement.MyBase.JumpthroughEvent.RemoveListener(Jumpthrough);
        }
    }

    private void Jumpthrough()
    {
        //Ignore collision
        col.enabled = false;
        //GloopMain.Instance.MyMovement.MyBase.OnJumpthroughPlatform--;
        //GloopMain.Instance.MyMovement.MyBase.JumpthroughEvent.RemoveListener(Jumpthrough);
        StartCoroutine(EnablePlaytform());
    }

    private IEnumerator EnablePlaytform()
    {
        yield return new WaitForSeconds(secondsForReactivation);
        col.enabled = true;
    }


}

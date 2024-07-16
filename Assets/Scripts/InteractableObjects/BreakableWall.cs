using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField]
    Collider2D myCol;
    bool enteredGround;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ProcessCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerFoot")
        {
            if (!ProcessCollision())
            {
                GloopMain.Instance.MyMovement.MyBase.GroundEnter();
                enteredGround = true;
            }
        }
    }

    private bool ProcessCollision()
    {
        if (GloopMain.Instance.CurrentMode != EMode.DASH)
            return false;
        GloopDash tmp = (GloopDash)GloopMain.Instance.MyMovement;
        if (tmp.IsDashing)
        {
            Break();
            return true;
        }
        else
        {
            tmp.DashEvent.AddListener(CheckForBreaking);
            return false;
        }
    }

    private void CheckForBreaking()
    {
        //GloopDash tmp = (GloopDash)GloopMain.Instance.MyMovement;
        //Vector3 op = transform.position
        StartCoroutine(ToggleCollision());
        //if (tmp.DashDir )
    }

    private IEnumerator ToggleCollision()
    {
        myCol.enabled = false;
        yield return new WaitForFixedUpdate();
        myCol.enabled = true;
    }

    private void Break()
    {
        //WwisePlay ObBlockBreak
        Backpack.Instance.LosableObjects.Add(gameObject);
        GloopDash tmp = (GloopDash)GloopMain.Instance.MyMovement;
        if (tmp != null)
        {
            tmp.DashEvent.RemoveListener(CheckForBreaking);
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ProcessCollisionExit(collision.collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerFoot")
        {
            ProcessCollisionExit(collision);
            if (enteredGround)
            {
                enteredGround = false;
                GloopMain.Instance.MyMovement.MyBase.GroundExit();
            }
        }
    }

    private void ProcessCollisionExit(Collider2D col)
    {
        GloopDash tmp = col.GetComponent<GloopDash>();
        if (tmp == null)
            return;
        else
        {
            tmp.DashEvent.RemoveListener(CheckForBreaking);
        }
    }
}

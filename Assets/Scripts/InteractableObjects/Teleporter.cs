using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    Teleporter destination;

    public Vector2 LockDir = new Vector2(1, 1);

    public Transform TeleportPos;

    public bool DisableTeleporter;

    private void Start()
    {
        //WwisePlay ObAmTeleporter
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && !DisableTeleporter)
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            DisableTeleporter = false;
        }
    }

    private void TeleportPlayer()
    {

        //WwisePlay ObTeleportPlayer
        var player = GloopMain.Instance.MyMovement.MyBase;
        player.transform.position = destination.TeleportPos.position;
        destination.DisableTeleporter = true;
        var rot = Quaternion.AngleAxis((destination.transform.eulerAngles.z - transform.eulerAngles.z) + 180, Vector3.forward);
        player.rb.velocity = rot * player.rb.velocity * destination.LockDir;
        player.VelocityToHold = rot * player.VelocityToHold * destination.LockDir;

        if (GloopMain.Instance.CurrentMode == EMode.DASH)
        {
            GloopDash tmp = (GloopDash)GloopMain.Instance.MyMovement;
            tmp.DashDir = rot * tmp.DashDir * destination.LockDir;
        }
    }
}

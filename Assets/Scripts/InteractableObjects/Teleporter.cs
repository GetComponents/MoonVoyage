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
        var player = GloopMain.Instance.MyMovement.MyBase;
        player.transform.position = destination.TeleportPos.position;
        destination.DisableTeleporter = true;
        var rot = Quaternion.AngleAxis((destination.transform.eulerAngles.z - transform.eulerAngles.z) + 180, Vector3.forward);
        //Debug.Log($"MyEA : {transform.eulerAngles.z}, targetEA {destination.transform.eulerAngles.z}; launch vector {rot * player.rb.velocity}");
        player.rb.velocity = rot * player.rb.velocity * destination.LockDir;
        player.VelocityToHold = rot * player.VelocityToHold * destination.LockDir;
        //player.rb.AddForce(rot * player.rb.velocity, ForceMode2D.Impulse);
        //GloopMain.Instance.MyMovement.MyBase.rb.AddForce(launchAngle * launchStrength);
        //GloopMain.Instance.Rotation.UnrotateCursor = false;
    }
}

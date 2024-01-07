using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleShot : MonoBehaviour
{
    public LineRenderer Lr;
    public GoopGrapple player;
    [SerializeField]
    float countdown;

    private void Start()
    {
        Lr.SetPosition(0, player.transform.position);
        Lr.SetPosition(1, transform.position);
        Lr.enabled = true;
    }
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Destroy(gameObject);
            Lr.enabled = false;
        }
        if ((!Input.GetKey(KeyCode.Mouse0)  && !Input.GetKey(KeyCode.Space)) || GloopMain.Instance.MyMovement != player)
        {
            Destroy(gameObject);
        }
        Lr.SetPosition(0, player.transform.position);
        Lr.SetPosition(1, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            ObjectProperty op;
            if (collision.TryGetComponent<ObjectProperty>(out op))
            {
                //tongueAnchorPos = op.CreateTonguePoint(transform.position);
                player.AttachPoint(op.CreateTonguePoint(transform.position));
            }
            else
            {
                player.AttachPoint(transform.position);
            }
            player.EnableTongue();
            Destroy(gameObject);
            //player.EnableTongue();
        }
    }
}

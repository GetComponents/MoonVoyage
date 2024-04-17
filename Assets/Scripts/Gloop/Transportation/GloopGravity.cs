using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloopGravity : GloopMove
{
    [SerializeField]
    AnimMethods spriteRotator;

    public override void AddMode()
    {
        MySoundtrack.volume = SoundtrackVolume;
        ModeSprite.color = ModeColor;
        //FlightDir = transform.right;
    }

    public override void MyUpdate()
    {
        if (!MyBase.InputLocked)
        {
            MyBase.MyUpdate();
            GravitySwitch();
        }
    }

    private void GravitySwitch()
    {
        if ((Input.GetKeyDown(KeyCode.Mouse0)) /*&& (MyBase.GroundedAmount > 0 || MyBase.JumpGracePeriod > 0)*/)
        {
            GameManager.Instance.GravitySwitch?.Invoke();
            MyBase.rb.gravityScale *= -1;
            spriteRotator.ChangeGravity();
            //transform.eulerAngles += new Vector3(0, 0, 180);
            //GloopMain.Instance.AimAnchor.eulerAngles -= new Vector3(0, 0, 180);
            //MyBase.GloopAnim.SetBool("Flying", true);

            //Vector4 tmp = ModeColor * (Mathf.Lerp(0.3f, 1, Mathf.Clamp(currentFlightTime, 0f, maxFlightTime) / maxFlightTime));
            //tmp.w = ModeColor.a;
            //ModeSprite.color = tmp;
        }
    }

    public override void RemoveMode()
    {
        this.enabled = false;
        //MySoundtrack.volume = 0;
        //FlySound.Stop();
        //MyBase.GloopAnim.SetBool("Flying", false);
        //MyBase.GloopAnim.SetBool("Walking", false);
        //MyBase.rb.gravityScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            EnterGround();
        }
    }

    public override void EnterGround()
    {
        MyBase.GroundEnter();
        if (GloopMain.Instance.MyMovement == this)
        {
            ModeSprite.color = ModeColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            ExitGround();
        }
    }

    public override void ExitGround()
    {
        MyBase.GroundExit();
    }
}

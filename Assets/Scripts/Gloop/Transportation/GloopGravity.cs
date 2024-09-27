using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GloopGravity : GloopMove
{
    [SerializeField]
    AnimMethods spriteRotator;

    private void Start()
    {
        GameManager.Instance.GravitySwitch.AddListener(GravitySwitch);
    }

    public override void AddMode()
    {
        MySoundtrack.volume = SoundtrackVolume;
        ModeSprite.color = ModeColor;
        //FlightDir = transform.right;
    }

    public override void MyUpdate()
    {
        MyBase.MyUpdate();
    }

    private void GravitySwitch()
    {
        MyBase.rb.gravityScale *= -1;
        spriteRotator.ChangeGravity();
        if (MyBase.rb.gravityScale > 0)
        {
            //WwisePlay PlChangeGravityDown
        }
        else
        {
            //WwisePlay PlChangeGravityUp
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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Floor")
    //    {
    //        EnterGround();
    //    }
    //}

    public override void EnterGround()
    {
        //MyBase.GroundEnter();
        if (GloopMain.Instance.MyMovement == this)
        {
            ModeSprite.color = ModeColor;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Floor")
    //    {
    //        ExitGround();
    //    }
    //}

    public override void ExitGround()
    {
        //MyBase.GroundExit();
    }

    public override void TriggerAbility(InputAction.CallbackContext context)
    {
        if (MyBase.InputLocked > 0 || MyBase.PauseLocked > 0)
            return;
        if (context.started)
        {
            GameManager.Instance.GravitySwitch?.Invoke();
        }
    }
}

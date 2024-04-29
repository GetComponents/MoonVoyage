using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GloopNone : GloopMove
{

    [SerializeField]
    AnimMethods spriteRotator;

    public override void AddMode()
    {
    }

    public override void MyUpdate()
    {
        if (!MyBase.InputLocked)
        {
            MyBase.MyUpdate();
        }
    }

    public override void RemoveMode()
    {
        this.enabled = false;
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
        if (MyBase.InputLocked)
            return;
    }
}

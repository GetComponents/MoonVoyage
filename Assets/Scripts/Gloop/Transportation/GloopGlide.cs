using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GloopGlide : GloopMove
{
    [SerializeField]
    float gravityScale;
    [SerializeField]
    float maxFlightTime;
    float currentFlightTime;
    [SerializeField]
    private float AirborneSpeed;
    private float defaultAirborneSpeed;
    [SerializeField]
    AudioSource FlySound;

    [SerializeField]
    private float counterGravity, minGravity;
    [SerializeField]
    private float sideSpeedMul;
    [SerializeField]
    private float jumpStrength;
    [SerializeField]
    bool infinteFlight;

    private bool pressingButton;
    //bool sideinput;


    public override void AddMode()
    {
        defaultAirborneSpeed = MyBase.airborneSpeed;
        MyBase.airborneSpeed = AirborneSpeed;
        if (MyBase.GroundedAmount <= 0)
        {
            MyBase.airborneMul = MyBase.airborneSpeed;
        }
        MySoundtrack.volume = SoundtrackVolume;
        ModeSprite.color = ModeColor;
        currentFlightTime = maxFlightTime;
        MyBase.rb.gravityScale = gravityScale;
        //FlightDir = transform.right;
    }

    public override void MyUpdate()
    {
        MyBase.MyUpdate();
        if (!(MyBase.InputLocked > 0 || MyBase.PauseLocked > 0))
        {
            VerticalInput();
        }
    }



    private void VerticalInput()
    {
        MyBase.rb.gravityScale = gravityScale;
        if (pressingButton && currentFlightTime > 0)
        {
            if (MyBase.rb.velocity.y < minGravity)
            {
                MyBase.rb.AddForce(MyBase.MovementDir.normalized * sideSpeedMul * ((MyBase.rb.velocity.y + minGravity) * (MyBase.rb.velocity.y + minGravity)) * Time.deltaTime, ForceMode2D.Force);
            }
            MyBase.GloopAnim.SetBool("Flying", true);
            //rb.gravityScale = upwardsForce;
            if (MyBase.rb.velocity.y < minGravity)
            {
                MyBase.rb.AddForce(new Vector2(0, counterGravity * (MyBase.rb.velocity.y + minGravity) * MyBase.rb.gravityScale), ForceMode2D.Force);
                //MyBase.rb.gravityScale = counterGravity * (MyBase.rb.velocity.y + minGravity);
            }
            if (!infinteFlight)
            {
                currentFlightTime -= Time.deltaTime;
                Vector4 tmp = ModeColor * (Mathf.Lerp(0.3f, 1, Mathf.Clamp(currentFlightTime, 0f, maxFlightTime) / maxFlightTime));
                tmp.w = ModeColor.a;
                ModeSprite.color = tmp;
            }
        }
        else
        {
            FlySound.Stop();
            MyBase.GloopAnim.SetBool("Flying", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentFlightTime > 0)
        {
            FlySound.Play();
        }
    }

    public override void RemoveMode()
    {
        MyBase.airborneSpeed = defaultAirborneSpeed;
        if (MyBase.GroundedAmount > 0)
        {
            MyBase.airborneMul = 1;
        }
        else
        {
            MyBase.airborneMul = MyBase.airborneSpeed;
        }
        MySoundtrack.volume = 0;
        FlySound.Stop();
        MyBase.GloopAnim.SetBool("Flying", false);
        MyBase.GloopAnim.SetBool("Walking", false);
        MyBase.rb.gravityScale = 1;
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
        currentFlightTime = maxFlightTime;
        if (GloopMain.Instance.MyMovement == this)
        {
            ModeSprite.color = ModeColor;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Floor")
    //        ExitGround();
    //}

    public override void ExitGround()
    {
        //MyBase.GroundExit();       
    }

    public override void TriggerAbility(InputAction.CallbackContext context)
    {
        if (MyBase.InputLocked > 0 || MyBase.PauseLocked > 0)
            return;
        pressingButton = context.performed;
        //if (context.performed && currentFlightTime > 0)
        //{
        //    if (MyBase.rb.velocity.y < minGravity)
        //    {
        //        MyBase.rb.AddForce(MyBase.MovementDir.normalized * sideSpeedMul * ((MyBase.rb.velocity.y + minGravity) * (MyBase.rb.velocity.y + minGravity)) * Time.deltaTime, ForceMode2D.Force);
        //    }
        //    MyBase.GloopAnim.SetBool("Flying", true);
        //    //rb.gravityScale = upwardsForce;
        //    if (MyBase.rb.velocity.y < minGravity)
        //    {
        //        MyBase.rb.AddForce(new Vector2(0, counterGravity * (MyBase.rb.velocity.y + minGravity) * MyBase.rb.gravityScale), ForceMode2D.Force);
        //        //MyBase.rb.gravityScale = counterGravity * (MyBase.rb.velocity.y + minGravity);
        //    }
        //    currentFlightTime -= Time.deltaTime;
        //    Vector4 tmp = ModeColor * (Mathf.Lerp(0.3f, 1, Mathf.Clamp(currentFlightTime, 0f, maxFlightTime) / maxFlightTime));
        //    tmp.w = ModeColor.a;
        //    ModeSprite.color = tmp;
        //}
        //else
        //{
        //    FlySound.Stop();
        //    MyBase.GloopAnim.SetBool("Flying", false);
        //}

        //if (context.started && currentFlightTime > 0)
        //{
        //    FlySound.Play();
        //}
    }
}

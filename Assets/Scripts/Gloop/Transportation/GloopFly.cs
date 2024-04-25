using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GloopFly : GloopMove
{
    [SerializeField]
    float gravityScale, upwardsForce, movementGrav;
    //[SerializeField]
    //float flySpeed;
    [SerializeField]
    float velocityDecrease;
    //public Vector2 FlightDir;
    //[SerializeField]
    //float lowEnd, highEnd, lowEndAcc, highEndAcc;
    [SerializeField]
    float maxFlightTime;
    float currentFlightTime;
    [SerializeField]
    AudioSource FlySound;




    public override void AddMode()
    {
        MySoundtrack.volume = SoundtrackVolume;
        ModeSprite.color = ModeColor;
        currentFlightTime = maxFlightTime;
        MyBase.rb.gravityScale = gravityScale;
        //FlightDir = transform.right;
    }

    public override void MyUpdate()
    {
        //if (Input.GetKey(KeyCode.D))
        //{
        //    rb.AddForce(FlightDir * Time.deltaTime * flySpeed / (rb.velocity.x * velocityDecrease + 1));
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    rb.AddForce(-FlightDir * Time.deltaTime * flySpeed / (rb.velocity.x * velocityDecrease + 1));
        //}
        if (!MyBase.InputLocked)
        {
            VerticalInput();
            MyBase.MyUpdate();
        }
    }

    private void VerticalInput()
    {
        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)) && currentFlightTime > 0)
        {
            MyBase.GloopAnim.SetBool("Flying", true);
            MyBase.rb.gravityScale = upwardsForce;
            currentFlightTime -= Time.deltaTime;
            Vector4 tmp = ModeColor * (Mathf.Lerp(0.3f, 1, Mathf.Clamp(currentFlightTime, 0f, maxFlightTime) / maxFlightTime));
            tmp.w = ModeColor.a;
            ModeSprite.color = tmp;
        }
        else
        {
            //if (sideinput == true && (MyBase.rb.velocity.y <= -1))
            //{
            //    //rb.velocity = new Vector2(rb.velocity.x + rb.velocity.y, 1);
            //    //rb.velocity.x += rb.velocity.y - 1;
            //    MyBase.rb.gravityScale = movementGrav;
            //}
            //else
            //{
            MyBase.rb.gravityScale = gravityScale;
            //}
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
        MySoundtrack.volume = 0;
        FlySound.Stop();
        MyBase.GloopAnim.SetBool("Flying", false);
        MyBase.GloopAnim.SetBool("Walking", false);
        MyBase.rb.gravityScale = 1;
        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            MyBase.GroundEnter();
            currentFlightTime = maxFlightTime;
            if (GloopMain.Instance.MyMovement == this)
            {
                ModeSprite.color = ModeColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
            ExitGround();
    }

    public override void EnterGround()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitGround()
    {
        MyBase.GroundExit();
    }

    public override void TriggerAbility(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}

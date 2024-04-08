using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloopGlide : GloopMove
{
    [SerializeField]
    float gravityScale;
    //[SerializeField]
    //float flySpeed;
    //[SerializeField]
    //float velocityDecrease;
    //public Vector2 FlightDir;
    //[SerializeField]
    //float lowEnd, highEnd, lowEndAcc, highEndAcc;
    [SerializeField]
    float maxFlightTime;
    float currentFlightTime;
    //Vector2 movementDir;
    //[SerializeField]
    //float groundedMultiplier;
    //float groundedMul;
    [SerializeField]
    AudioSource FlySound;

    [SerializeField]
    private float counterGravity, minGravity;
    [SerializeField]
    private float sideSpeedMul;
    [SerializeField]
    private float jumpStrength;
    //bool sideinput;


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
        if (!MyBase.InputLocked)
        {
            MyBase.MyUpdate();
            VerticalInput();
        }
    }



    private void VerticalInput()
    {
        MyBase.rb.gravityScale = gravityScale;
        if ((Input.GetKey(KeyCode.Mouse0)) && currentFlightTime > 0)
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
            currentFlightTime -= Time.deltaTime;
            Vector4 tmp = ModeColor * (Mathf.Lerp(0.3f, 1, Mathf.Clamp(currentFlightTime, 0f, maxFlightTime) / maxFlightTime));
            tmp.w = ModeColor.a;
            ModeSprite.color = tmp;
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
        MySoundtrack.volume = 0;
        FlySound.Stop();
        MyBase.GloopAnim.SetBool("Flying", false);
        MyBase.GloopAnim.SetBool("Walking", false);
        MyBase.rb.gravityScale = 1;
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
        MyBase.GroundExit();
    }
}
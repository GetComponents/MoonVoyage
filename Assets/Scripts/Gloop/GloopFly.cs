using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloopFly : GloopMove
{
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    float gravityScale, upwardsForce, movementGrav;
    [SerializeField]
    float flySpeed;
    [SerializeField]
    float velocityDecrease;
    public Vector2 FlightDir;
    [SerializeField]
    float lowEnd, highEnd, lowEndAcc, highEndAcc;
    [SerializeField]
    float maxFlightTime;
    float currentFlightTime;
    Vector2 movementDir;
    [SerializeField]
    float groundedMultiplier;
    float groundedMul;
    [SerializeField]
    AudioSource FlySound;


    bool sideinput;


    public override void AddMode()
    {
        MySoundtrack.volume = SoundtrackVolume;
        ModeSprite.color = ModeColor;
        currentFlightTime = maxFlightTime;
        rb.gravityScale = gravityScale;
        FlightDir = transform.right;
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
        if (!InputLocked)
        {
            SideInput();
            VerticalInput();
        }
    }

    private void VerticalInput()
    {
        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)) && currentFlightTime > 0)
        {
            GloopAnim.SetBool("Flying", true);
            rb.gravityScale = upwardsForce;
            currentFlightTime -= Time.deltaTime;
            Vector4 tmp = ModeColor * (Mathf.Lerp(0.3f, 1, Mathf.Clamp(currentFlightTime, 0f, maxFlightTime) / maxFlightTime));
            tmp.w = ModeColor.a;
            //Debug.Log(tmp);
            //debugSprite.color = defaultColor * (currentFlightTime / maxFlightTime);
            ModeSprite.color = tmp;

        }
        else
        {
            if (sideinput == true && (rb.velocity.y <= -1))
            {
                //rb.velocity = new Vector2(rb.velocity.x + rb.velocity.y, 1);
                //rb.velocity.x += rb.velocity.y - 1;
                rb.gravityScale = movementGrav;
            }
            else
            {
                rb.gravityScale = gravityScale;
            }
            FlySound.Stop();
            GloopAnim.SetBool("Flying", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentFlightTime > 0)
        {
            FlySound.Play();
        }
    }

    private void SideInput()
    {
        movementDir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            if (rb.velocity.x > lowEnd)
            {
                movementDir.x -= 1 * lowEndAcc;
            }
            else if (rb.velocity.x < -highEnd)
            {
                movementDir.x -= 1 * highEndAcc;
            }
            else
            {
                movementDir.x -= 1;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (rb.velocity.x < -lowEnd)
            {
                movementDir.x += 1 * lowEndAcc;
            }
            else if (rb.velocity.x > highEnd)
            {
                movementDir.x += 1 * highEndAcc;
            }
            else
            {
                movementDir.x += 1;
            }
        }
        rb.AddForce(movementDir * flySpeed * Time.deltaTime * groundedMul, ForceMode2D.Force);
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.S))
        {
            sideinput = true;
            GloopAnim.SetBool("Walking", true);

        }
        else
        {
            sideinput = false;
            GloopAnim.SetBool("Walking", false);

        }
    }

    public override void RemoveMode()
    {
        MySoundtrack.volume = 0;
        FlySound.Stop();
        GloopAnim.SetBool("Flying", false);
        GloopAnim.SetBool("Walking", false);
        rb.gravityScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            groundedMul = groundedMultiplier;
            currentFlightTime = maxFlightTime;
            groundedAmount++;
            if (GloopMain.Instance.MyMovement == this)
            {
                GloopAnim.SetBool("Grounded", true);
                ModeSprite.color = ModeColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            groundedAmount--;
            if (groundedAmount == 0)
            {
                groundedMul = 1;
                if (GloopMain.Instance.MyMovement == this)
                    GloopAnim.SetBool("Grounded", false);
            }
            if (groundedAmount < 0)
            {
                Debug.LogWarning("Grounded Amount below 0!!");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopWalk : GloopMove
{
    [SerializeField]
    float movementSpeed;
    public int DashCharge;
    [SerializeField]
    float lookSensitivity;
    private Vector3 dashDir;
    [SerializeField]
    Rigidbody2D rb;
    Vector2 movementDir;
    [SerializeField]
    float lowEnd, highEnd, lowEndAcc, highEndAcc, AirborneSpeed;
    float airborneMul;
    [SerializeField]
    float dashStrength;
    //[SerializeField]
    //Transform CursorSignifier;
    //[SerializeField]
    //Transform aimPoint;
    [SerializeField]
    float dashEndTimer;
    bool canDash = true, isDashing;

    [SerializeField]
    float JumpGracePeriod;
    float gracePeriod;
    [SerializeField]
    AnimMethods rotateSprite;
    [SerializeField]
    float oneChargeColor, noChargeColor;
    int lastDashSound;
    [SerializeField]
    AudioClip DashSound1, DashSound2, DashSound3;

    public override void AddMode()
    {
        if (groundedAmount == 0)
        {
            Vector4 tmp = ModeColor * oneChargeColor;
            tmp.w = 1;
            ModeSprite.color = tmp;
        }
        else
        {
            ModeSprite.color = ModeColor;
        }
        MySoundtrack.volume = SoundtrackVolume;
        GloopMain.Instance.firePoint.GetComponent<SpriteRenderer>().enabled = true;
        DashCharge++;
    }
    public override void MyUpdate()
    {
        //DirectionalInput();
        //Debug.Log("Updating");
        if (!InputLocked)
        {
            GroundMovement();
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
            if (isDashing)
            {
                rotateSprite.RotateCharacter();
                GloopAnim.SetBool("Dashing", true);
                rb.velocity = Vector3.Normalize(dashDir) * dashStrength;
            }
            if (rb.velocity.y <= 0)
            {
                rotateSprite.rotate = false;
            }
            gracePeriod -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (DashCharge > 0 && canDash)
        {
            if (groundedAmount == 0 && gracePeriod < 0)
            {
                DashCharge--;
                if (DashCharge == 0)
                {
                    Vector4 tmp = ModeColor * noChargeColor;
                    tmp.w = 1;
                    ModeSprite.color = tmp;
                }
            }
            PlayRandomDashSound();
            canDash = false;
            isDashing = true;
            //rb.velocity = Vector3.zero;
            dashDir = GloopMain.Instance.firePoint.localPosition;
            rb.AddForce(Vector3.Normalize(dashDir) * dashStrength);
            StartCoroutine(DashEnd());
        }

    }

    private void PlayRandomDashSound()
    {
        int tmp = Random.Range(0, 3);
        while (lastDashSound == tmp)
        {
            tmp = Random.Range(0, 3);
        }
        switch (tmp)
        {
            case 0:
                SoundManager.Instance.PlayEffect(DashSound1);
                //AudioSource.PlayClipAtPoint(DashSound1, transform.position);
                break;
            case 1:
                SoundManager.Instance.PlayEffect(DashSound2);
                //AudioSource.PlayClipAtPoint(DashSound2, transform.position);
                break;
            case 2:
                SoundManager.Instance.PlayEffect(DashSound3);
                //AudioSource.PlayClipAtPoint(DashSound3, transform.position);
                break;
            default:
                break;
        }
        lastDashSound = tmp;
    }

    private IEnumerator DashEnd()
    {
        yield return new WaitForSeconds(dashEndTimer);
        canDash = true;
        isDashing = false;
        GloopAnim.SetBool("Dashing", false);
    }

    //private void DirectionalInput()
    //{
    //    //Cursor.lockState = CursorLockMode.Locked;
    //    lookPoint.x += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;

    //    lookPoint.y += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

    //    lookPoint.x = CheckBoundries(lookPoint.x, -1, 1);
    //    lookPoint.y = CheckBoundries(lookPoint.y, -1, 1);
    //    //Debug.Log(lookPoint);
    //    aimPoint.transform.localPosition = Vector3.Normalize(lookPoint);

    //}

    private void GroundMovement()
    {
        movementDir = Vector2.zero;
        //Vector2 currentVelocity = rb.velocity;

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
        rb.AddForce(movementDir * movementSpeed * airborneMul * Time.deltaTime, ForceMode2D.Force);


        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            GloopAnim.SetBool("Walking", true);
        }
        else
        {
            GloopAnim.SetBool("Walking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Object tag " + collision.tag);
        if (collision.tag == "Floor")
        {
            groundedAmount++;
            airborneMul = 1;
            DashCharge = 1;
            if (GloopMain.Instance.MyMovement == this)
            {
                ModeSprite.color = ModeColor;
                GloopAnim.SetBool("Grounded", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            //Debug.Log("Grounded amount: " + groundedAmount);
            groundedAmount--;
            if (groundedAmount == 0)
            {
                GloopAnim.SetBool("Grounded", false);
                airborneMul = AirborneSpeed;
                gracePeriod = JumpGracePeriod;
                if (GloopMain.Instance.MyMovement == this)
                {
                    Vector4 tmp = ModeColor * oneChargeColor;
                    tmp.w = 1;
                    ModeSprite.color = tmp;
                }
            }
        }
    }

    public override void RemoveMode()
    {
        MySoundtrack.volume = 0;
        GloopAnim.SetBool("Dashing", false);
        GloopAnim.SetBool("Walking", false);
        canDash = true;
        isDashing = false;
        GloopMain.Instance.firePoint.GetComponent<SpriteRenderer>().enabled = false;
    }
}

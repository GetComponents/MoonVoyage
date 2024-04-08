using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopWalk : GloopMove
{
    //[SerializeField]
    //float movementSpeed;
    public int DashCharge;
    [SerializeField]
    float lookSensitivity;
    private Vector3 dashDir;
    //Vector2 movementDir;
    //[SerializeField]
    //float lowEnd, highEnd, lowEndAcc, highEndAcc, AirborneSpeed;
    //float airborneMul;
    [SerializeField]
    float dashStrength;
    //[SerializeField]
    //Transform CursorSignifier;
    //[SerializeField]
    //Transform aimPoint;
    [SerializeField]
    float dashEndTimer;
    bool canDash = true, isDashing;

    //[SerializeField]
    //float JumpGracePeriod;
    //float gracePeriod;
    [SerializeField]
    AnimMethods rotateSprite;
    [SerializeField]
    float oneChargeColor, noChargeColor;
    int lastDashSound;
    [SerializeField]
    AudioClip DashSound1, DashSound2, DashSound3;

    public override void AddMode()
    {
        if (MyBase.GroundedAmount == 0)
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
        if (!MyBase.InputLocked)
        {
            MyBase.MyUpdate();
            //GroundMovement();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Dash();
            }
            if (isDashing)
            {
                rotateSprite.RotateCharacter();
                MyBase.GloopAnim.SetBool("Dashing", true);
                MyBase.rb.velocity = Vector3.Normalize(dashDir) * dashStrength;
            }
            if (MyBase.rb.velocity.y <= 0)
            {
                rotateSprite.rotate = false;
            }
            //racePeriod -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (DashCharge > 0 && canDash)
        {
            if (MyBase.GroundedAmount == 0)
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
            MyBase.rb.velocity = Vector3.zero;
            dashDir = GloopMain.Instance.firePoint.localPosition;
            MyBase.rb.AddForce(Vector3.Normalize(dashDir) * dashStrength);
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
        MyBase.GloopAnim.SetBool("Dashing", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Object tag " + collision.tag);
        if (collision.tag == "Floor")
        {
            MyBase.GroundEnter();
            DashCharge = 1;
            if (GloopMain.Instance.MyMovement == this)
            {
                ModeSprite.color = ModeColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            MyBase.GroundExit();
            if (MyBase.GroundedAmount == 0 && GloopMain.Instance.MyMovement == this)
            {
                Vector4 tmp = ModeColor * oneChargeColor;
                tmp.w = 1;
                ModeSprite.color = tmp;
            }
        }
    }

    public override void RemoveMode()
    {
        MySoundtrack.volume = 0;
        MyBase.GloopAnim.SetBool("Dashing", false);
        MyBase.GloopAnim.SetBool("Walking", false);
        canDash = true;
        isDashing = false;
        GloopMain.Instance.firePoint.GetComponent<SpriteRenderer>().enabled = false;
    }
}

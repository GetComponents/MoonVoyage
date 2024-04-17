using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GloopMoveBase : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool InputLocked;
    public Vector2 MovementDir;
    public float movementSpeed, airborneSpeed;
    public float airborneMul;
    [SerializeField]
    private float lowEnd, highEnd, lowEndAcc, highEndAcc;
    [SerializeField]
    public float JumpStrength, JumpGracePeriod;
    [HideInInspector]
    public float GracePeriod;
    public Animator GloopAnim;
    bool stickingToSurface;
    public UnityEvent StickToSurfaceEvent, UnstickToSurfaceEvent;
    public bool jumped;
    public bool HoldingVelocity;
    public Vector2 VelocityToHold;

    public int GroundedAmount
    {
        get => m_groundedAmount;
        set
        {
            if (value < 0)
            {
                m_groundedAmount = 0;
            }
            else
            {
                m_groundedAmount = value;
            }
        }
    }
    [SerializeField]
    int m_groundedAmount;

    private void GroundMovement()
    {
        MovementDir = Vector2.zero;
        //Vector2 currentVelocity = rb.velocity;

        if (Input.GetKey(KeyCode.A))
        {
            if (rb.velocity.x > lowEnd)
            {
                MovementDir.x -= 1 * lowEndAcc;
            }
            else if (rb.velocity.x < -highEnd)
            {
                MovementDir.x -= 1 * highEndAcc;
            }
            else
            {
                MovementDir.x -= 1;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (rb.velocity.x < -lowEnd)
            {
                MovementDir.x += 1 * lowEndAcc;
            }
            else if (rb.velocity.x > highEnd)
            {
                MovementDir.x += 1 * highEndAcc;
            }
            else
            {
                MovementDir.x += 1;
            }
        }
        rb.AddForce(MovementDir * movementSpeed * airborneMul * Time.deltaTime, ForceMode2D.Force);


        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            GloopAnim.SetBool("Walking", true);
        }
        else
        {
            GloopAnim.SetBool("Walking", false);
        }
    }

    public void MyUpdate()
    {
        if (HoldingVelocity)
            HoldVelocity();
        if (InputLocked)
            return;
        CheckJump();
        GroundMovement();
    }

    private void CheckJump()
    {
        GracePeriod -= Time.deltaTime;
        if (jumped && GroundedAmount == 0)
        {
            //if (rb.velocity.y * GloopMain.Instance.Rotation.Gravity >= -0.1f)
            //    GloopMain.Instance.Rotation.RotatePlayerToRotation(new Vector3(0, 0, 180));
            //else
            //{
            //    jumped = false;
            //    //GloopMain.Instance.Rotation.RotateToGravity();
            //}
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stickingToSurface)
            {
                Unstick();
                return;
            }
            if ((GroundedAmount > 0 || GracePeriod > 0))
            {
                Jump();
            }
        }
    }

    public void Jump()
    {
        jumped = true;
        GracePeriod = 0;
        rb.AddForce(new Vector2(0, JumpStrength * rb.gravityScale));
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (HoldingVelocity)
    //    {
    //        //rb.gravityScale = GloopMain.Instance.Rotation.Gravity;
    //        VelocityToHold = Vector2.zero;
    //        HoldingVelocity = false;
    //        InputLocked = false;
    //    }
    //}

    public void GroundEnter()
    {
        GloopMain.Instance.Rotation.RotateToGravity();
        GroundedAmount++;
        airborneMul = 1;
        GloopAnim.SetBool("Grounded", true);
    }

    public void GroundExit()
    {
        GroundedAmount--;
        if (GroundedAmount == 0)
        {
            GloopAnim.SetBool("Grounded", false);
            airborneMul = airborneSpeed;
            GracePeriod = JumpGracePeriod;
        }
    }

    public void HoldVelocity()
    {
        rb.velocity = VelocityToHold;
        //rb.AddForce(VelocityToHold);
        //Debug.Log("rb velocity: " + rb.velocity);
    }

    public void StickToSurface(Transform surface)
    {
        StickToSurfaceEvent?.Invoke();
        rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
        rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        ParentToPlatform(surface);
        stickingToSurface = true;
    }

    public void ParentToPlatform(Transform surface)
    {
        transform.SetParent(surface);
    }

    public void Unstick()
    {
        stickingToSurface = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        UnparentFromPlatform();
        UnstickToSurfaceEvent?.Invoke();
    }

    public void UnparentFromPlatform()
    {
        transform.SetParent(GloopMain.Instance.transform);
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
    }
}

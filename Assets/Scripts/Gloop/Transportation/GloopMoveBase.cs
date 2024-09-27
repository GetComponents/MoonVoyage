using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GloopMoveBase : MonoBehaviour
{
    public Rigidbody2D rb;
    public int InputLocked;
    public int PauseLocked;
    public Vector2 MovementDir;
    public float movementSpeed, airborneSpeed;
    public float airborneMul;
    [SerializeField]
    private float lowEnd, highEnd, lowEndAcc, midEndAcc, highEndAcc;
    public float MinMovement;
    [SerializeField]
    public float JumpStrength, extraJumpStrength, JumpGracePeriod;
    [SerializeField]
    float maxJumpDuration;
    float jumpTimer;
    [HideInInspector]
    public float GracePeriod;
    public Animator GloopAnim;
    bool stickingToSurface;
    public UnityEvent StickToSurfaceEvent, UnstickToSurfaceEvent;
    public bool jumped;
    public bool HoldingVelocity
    {
        get => m_holdingVelocity;
        set
        {
            //Debug.Log("Holding velocity is " + value);
            if (value != m_holdingVelocity)
            {
                if (value)
                {
                    //WwisePlay PlFlyingAroundLoop
                }
                else
                {
                    //WwiseStopPlay PlFlyingAroundLoop
                }
            }
            m_holdingVelocity = value;
        }
    }
    private bool m_holdingVelocity;
    public Vector2 VelocityToHold;
    public Vector2 CurrentMovement;
    [SerializeField]
    float friction;
    [HideInInspector]
    public int OnJumpthroughPlatform;
    [HideInInspector]
    public UnityEvent JumpthroughEvent, ExternalAddforce = new UnityEvent();

    public int GroundedAmount
    {
        get => m_groundedAmount;
        set
        {
            if (value < 0)
            {
                //WwiseStopPlay PlFootstepLoop
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

    public void NewMove(InputAction.CallbackContext context)
    {
        //if (context.phase)
        if (Mathf.Approximately(CurrentMovement.x, 0) && !Mathf.Approximately(context.ReadValue<Vector2>().x, 0))
        {
            if (GroundedAmount > 0)
            {
                //WwisePlay PlFootstepLoop (extra logic for different kinds of footstep missing)
            }
        }
        if (Mathf.Approximately(context.ReadValue<Vector2>().x, 0))
        {
            //WwiseStopPlay PlFootstepLoop
        }
        CurrentMovement = context.ReadValue<Vector2>();
        MovementDir.y = CurrentMovement.y;

        //if (CurrentMovement.x != 0)
        //{
        //}
        //else
        //{
        //}
        //GroundMovement(context.ReadValue<Vector2>());
    }

    private void GroundMovement(Vector2 movement)
    {
        if (movement.x <= MinMovement && movement.x >= -MinMovement)
        {
            MovementDir.x = 0;
            GloopAnim.SetBool("Walking", false);
        }
        else
        {
            GloopAnim.SetBool("Walking", true);
            MovementDir = Vector2.zero;
            if (Mathf.Abs(rb.velocity.x) < lowEnd * Mathf.Abs(movement.x) || movement.x * rb.velocity.x < 0)
            {
                MovementDir.x += movement.x * lowEndAcc;
            }
            else if (Mathf.Abs(rb.velocity.x) > highEnd * Mathf.Abs(movement.x))
            {
                MovementDir.x += movement.x * highEndAcc;
            }
            else
            {
                MovementDir.x += movement.x * midEndAcc;
            }
            rb.AddForce(MovementDir * movementSpeed * airborneMul * Time.deltaTime, ForceMode2D.Force);
        }


        //if (movement.x != 0)
        //{
        //    GloopAnim.SetBool("Walking", true);
        //}
        //else
        //{
        //    GloopAnim.SetBool("Walking", false);
        //}
    }

    private void Start()
    {
        GameManager.Instance.GravitySwitch.AddListener(MoveHitbox);
        UnstickToSurfaceEvent.AddListener(Unstick);
    }

    public void MyUpdate()
    {
        if (HoldingVelocity)
            HoldVelocity();
        if (InputLocked > 0 || PauseLocked > 0)
            return;
        ExtraJumpHeight();
        GracePeriod -= Time.deltaTime;
        //if (CurrentMovement != Vector2.zero)
        //{
        //if (GameManager.Instance.InputType == 0)
        //{
        GroundMovement(CurrentMovement);
        //}
        //else
        //{

        //}
        //GroundMovement();
        //}
        //if (GroundedAmount != 0 && Mathf.Abs(rb.velocity.x) > 0.001f && Mathf.Abs(rb.velocity.x) > highEnd)
        //{
        //}
        if (GroundedAmount != 0 && ((MovementDir.x != 0 && Mathf.Abs(rb.velocity.x) > highEnd) || (MovementDir.x == 0 && Mathf.Abs(rb.velocity.x) > 0.0001f)))
        {
            Friction();
        }
    }

    private void ExtraJumpHeight()
    {
        if (jumped)
        {
            jumpTimer -= Time.deltaTime;
            rb.AddForce(new Vector2(0, extraJumpStrength * (jumpTimer / maxJumpDuration) * rb.gravityScale * Time.deltaTime), ForceMode2D.Force);
            if (jumpTimer <= 0)
            {
                jumped = false;
            }
        }
    }

    private void Friction()
    {
        rb.AddForce(new Vector2(rb.velocity.x * -friction * Time.deltaTime, 0));
        //Debug.Log(rb.velocity * -friction);
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (stickingToSurface)
            {
                Unstick();
                return;
            }
            if (OnJumpthroughPlatform > 0 && MovementDir.y < -0.6f)
            {
                JumpthroughEvent.Invoke();
                return;
            }
            if ((GroundedAmount > 0 || GracePeriod > 0))
            {
                Jump();
            }
        }
        else if (context.canceled)
        {
            jumped = false;
            jumpTimer = maxJumpDuration;
        }
    }

    public void Jump()
    {
        //Debug.Log(GroundedAmount + " " + GracePeriod);
        //WwisePlay PlJump
        jumped = true;
        GracePeriod = 0;
        Vector2 tmp = rb.velocity;
        tmp.y = 0;
        rb.velocity = tmp;
        rb.AddForce(new Vector2(0, JumpStrength * rb.gravityScale), ForceMode2D.Impulse);
    }

    public void AddForce(Vector2 direction)
    {
        rb.AddForce(direction);
        ExternalAddforce.Invoke();
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
        jumped = false;
        GloopMain.Instance.Rotation.RotateToGravity();
        GroundedAmount++;
        airborneMul = 1;
        GloopAnim.SetBool("Grounded", true);
        GloopMain.Instance.MyMovement.EnterGround();
        if (HoldingVelocity == true)
        {
            HoldingVelocity = false;
            InputLocked = 0;
        }
    }

    public void GroundExit()
    {
        GroundedAmount--;
        GloopMain.Instance.MyMovement.ExitGround();
        if (GroundedAmount == 0)
        {
            GloopAnim.SetBool("Grounded", false);
            airborneMul = airborneSpeed;
            if (jumped == false)
            {
                GracePeriod = JumpGracePeriod;
            }
            //GracePeriod = JumpGracePeriod;
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
        //WwisePlay PlStickToStickySurface
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
        //WwisePlay PlUnstickFromStickySurface
        stickingToSurface = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        UnparentFromPlatform();
        //UnstickToSurfaceEvent?.Invoke();
    }

    public void UnparentFromPlatform()
    {
        Debug.Log("Unparented");
        transform.SetParent(GloopMain.Instance.transform);
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    private void MoveHitbox()
    {
        GetComponent<BoxCollider2D>().offset *= new Vector2(1, -1);
    }
}

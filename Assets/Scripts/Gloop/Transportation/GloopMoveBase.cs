using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (InputLocked)
            return;
        Jump();
        GroundMovement();
    }

    private void Jump()
    {
        GracePeriod -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && (GroundedAmount > 0 || GracePeriod > 0))
        {
            GracePeriod = 0;
            rb.AddForce(new Vector2(0, JumpStrength * rb.gravityScale));
        }
    }

    public void GroundEnter()
    {
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
}

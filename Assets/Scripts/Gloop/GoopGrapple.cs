using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopGrapple : GloopMove
{
    [SerializeField]
    float movementSpeed, airborneSpeed;
    float airborneMul;
    bool tongueEnabled, extendingTongue;
    
    [Header("Tongue Stuff:")]
    [SerializeField]
    SpringJoint2D tongue;
    private Transform firePoint => GloopMain.Instance.firePoint;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    LineRenderer tongueRender;
    TonguePoint tongueAnchorPos;
    [SerializeField]
    TonguePoint TonguePoint;
    ObjectProperty hitObj;
    [SerializeField]
    GameObject gloopShot;
    GameObject currentShot;
    [SerializeField]
    float shotSpeed;
    Vector2 lookPoint;
    [SerializeField]
    float lookSensitivity;
    [SerializeField]
    float aimRadius;
    [SerializeField]
    float lowEnd, highEnd, lowEndAcc, highEndAcc;
    Vector2 movementDir;
    [SerializeField]
    float retractSpeed;
    [SerializeField]
    float tonguePointOffset;

    [SerializeField]
    AudioClip shotSound;

    public override void MyUpdate()
    {
        if (InputLocked)
            return;
        Movement();
        //DirectionalInput();
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            RaycastToMousePos();
        }
        if (extendingTongue)
        {
            //ExtendTongue();
        }
        if (tongueEnabled)
        {
            UpdateTongue();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            DisableTongue();
        }
    }

    private void Movement()
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
        rb.AddForce(movementDir * movementSpeed * Time.deltaTime * airborneMul, ForceMode2D.Force);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            GloopAnim.SetBool("Walking", true);
        }
        else
        {
            GloopAnim.SetBool("Walking", false);
        }
        //Debug.Log("movement " + (movementDir * movementSpeed * Time.deltaTime));
    }

    private void RaycastToMousePos()
    {
        SoundManager.Instance.PlayEffect(shotSound);
        //AudioSource.PlayClipAtPoint(shotSound, transform.position);
        currentShot = Instantiate(gloopShot, firePoint.position, Quaternion.identity);
        currentShot.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(firePoint.localPosition) * shotSpeed);
        currentShot.GetComponent<GrappleShot>().player = this;
        currentShot.GetComponent<GrappleShot>().Lr = tongueRender;
    }

    private void UpdateTongue()
    {
        GloopAnim.SetBool("Grappling", true);
        tongue.distance -= retractSpeed * Time.deltaTime;
        tongueRender.SetPosition(0, TonguePoint.Position + Vector3.Normalize(TonguePoint.Position - tongue.transform.position) * tonguePointOffset);
        tongueRender.SetPosition(1, tongue.transform.position );
    }

    private void CheckForInterruption()
    {
        Vector3 distance = new Vector3(tongueAnchorPos.Position.x - firePoint.position.x, tongueAnchorPos.Position.y - firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector3.Normalize(distance), distance.magnitude - 0.1f);
        Debug.DrawRay(firePoint.position, Vector3.Normalize(distance) * (distance.magnitude - 0.1f), Color.green, 0.00f);
        if (hit.collider != null)
        {
            ObjectProperty op;
            if (hit.collider.TryGetComponent<ObjectProperty>(out op))
            {
                tongueAnchorPos = op.CreateTonguePoint(hit.point);
                AttachPoint(tongueAnchorPos);
            }
            else
            {
                AttachPoint(hit.point);
            }
        }
    }

    public void EnableTongue()
    {
        tongue.enabled = true;
        tongueEnabled = true;
        tongueRender.enabled = true;
        //tongue.autoConfigureDistance = false;
    }

    public void AttachPoint(TonguePoint point)
    {
        tongue.connectedBody = null;
        tongue.connectedAnchor = Vector2.zero;
        if (point.IsStatic)
        {
            tongue.connectedAnchor = point.Position;
            tongue.distance = new Vector3(point.Position.x - firePoint.position.x, point.Position.y - firePoint.position.y).magnitude;
        }
        else
        {
            tongue.connectedBody = point.LilDotRB;
            tongue.distance = new Vector3(point.LilDotPos.position.x - firePoint.position.x, point.LilDotPos.transform.position.y - firePoint.position.y).magnitude;
        }
        tongueAnchorPos = point;
        TonguePoint = point;
    }

    public void AttachPoint(Vector2 point)
    {
        tongue.connectedBody = null;
        tongue.connectedAnchor = Vector2.zero;
        tongueAnchorPos = new TonguePoint(point);
        tongue.connectedAnchor = tongueAnchorPos.Position;
        tongue.distance = new Vector3(tongueAnchorPos.Position.x - firePoint.position.x, tongueAnchorPos.Position.y - firePoint.position.y).magnitude;
        TonguePoint = tongueAnchorPos;
    }

    public void DisableTongue()
    {
        GloopAnim.SetBool("Grappling", false);
        tongueAnchorPos = null;
        tongueRender.enabled = false;
        //for (int i = 0; i < tongueSegments.Length; i++)
        //{
        //    tongueSegments[i].enabled = false;
        //}
        TonguePoint = null;

        tongue.enabled = false;
        tongueEnabled = false;
        //tongue.autoConfigureDistance = true;
        tongue.connectedBody = null;
        tongue.connectedAnchor = Vector2.zero;
        if (hitObj != null && hitObj.Moves)
        {
            hitObj.KillLilDot();
        }
        hitObj = null;
    }


    public override void RemoveMode()
    {
        MySoundtrack.volume = 0;
        GloopAnim.SetBool("Walking", false);
        GloopAnim.SetBool("Grappling", false);
        firePoint.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(currentShot);
        DisableTongue();
    }

    public override void AddMode()
    {
        MySoundtrack.volume = SoundtrackVolume;
        ModeSprite.color = ModeColor;
        firePoint.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Object tag " + collision.tag);
        if (collision.tag == "Floor")
        {
            airborneMul = 1;
            groundedAmount++;
            GloopAnim.SetBool("Grounded", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            airborneMul = airborneSpeed;
            groundedAmount--;
            if (groundedAmount == 0)
            {
                GloopAnim.SetBool("Grounded", false);
            }
        }
    }

}
public class TonguePoint
{
    public TonguePoint(Vector2 position)
    {
        IsStatic = true;
        Position = position;
    }
    public TonguePoint(Rigidbody2D rb, Transform lilDotPos)
    {
        IsStatic = false;
        LilDotPos = lilDotPos;
        LilDotRB = rb;
    }

    public bool IsStatic;
    public Vector3 Position
    {
        get
        {
            if (IsStatic)
            {
                return g_position;
            }
            else
            {
                return LilDotPos.position;
            }
        }
        set => g_position = value;
    }
    public Transform LilDotPos;
    public Rigidbody2D LilDotRB;
    private Vector3 g_position;
}

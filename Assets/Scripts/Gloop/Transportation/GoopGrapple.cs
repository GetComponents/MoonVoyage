using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopGrapple : GloopMove
{
    //[SerializeField]
    //float movementSpeed, airborneSpeed;
    //float airborneMul;
    bool tongueEnabled, extendingTongue;

    [Header("Tongue Stuff:")]
    [SerializeField]
    SpringJoint2D tongue;
    private Transform firePoint => GloopMain.Instance.firePoint;
    [SerializeField]
    LineRenderer tongueRender;
    TonguePoint tongueAnchorPos;
    [SerializeField]
    TonguePoint TonguePoint;
    //ObjectProperty hitObj;
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
    //[SerializeField]
    //float lowEnd, highEnd, lowEndAcc, highEndAcc;
    //Vector2 movementDir;
    [SerializeField]
    float retractSpeed;
    [SerializeField]
    float tonguePointOffset;
    bool stickingToSurface;

    [SerializeField]
    AudioClip shotSound;

    private void Start()
    {
        MyBase.StickToSurfaceEvent.AddListener(DisableTongue);
    }

    public override void MyUpdate()
    {
        if (MyBase.InputLocked)
            return;
        MyBase.MyUpdate();
        //DirectionalInput();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastToMousePos();
        }
        if (stickingToSurface)
        {
            return;
        }
        if (extendingTongue)
        {
            //ExtendTongue();
        }
        if (tongueEnabled)
        {
            UpdateTongue();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DisableTongue();
        }
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
        MyBase.GloopAnim.SetBool("Grappling", true);
        tongue.distance -= retractSpeed * Time.deltaTime;
        tongueRender.SetPosition(0, TonguePoint.Position + Vector3.Normalize(TonguePoint.Position - tongue.transform.position) * tonguePointOffset);
        tongueRender.SetPosition(1, tongue.transform.position);
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
        MyBase.GloopAnim.SetBool("Grappling", false);
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
        //if (hitObj != null && hitObj.Moves)
        //{
        //    hitObj.KillLilDot();
        //}
        //hitObj = null;
    }


    public override void RemoveMode()
    {
        MySoundtrack.volume = 0;
        MyBase.GloopAnim.SetBool("Walking", false);
        MyBase.GloopAnim.SetBool("Grappling", false);
        firePoint.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(currentShot);
        DisableTongue();
        this.enabled = false;
    }

    public override void AddMode()
    {
        MySoundtrack.volume = SoundtrackVolume;
        ModeSprite.color = ModeColor;
        firePoint.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor")
        {
            EnterGround();
        }
    }

    public override void EnterGround()
    {
        MyBase.GroundEnter();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor")
        {
            ExitGround();
        }
    }

    public override void ExitGround()
    {
        MyBase.GroundExit();
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

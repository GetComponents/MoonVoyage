using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperty : MonoBehaviour
{
    public bool Moves, Rotates;
    //public bool movable;
    [SerializeField]
    public ObjectType Type;
    //public float stickiness;
    [SerializeField, Header("Launch")]
    float launchStrength;
    [SerializeField]
    Vector2 LockDir = new Vector2(1, 1);
    [SerializeField]
    GameObject lilDot;
    public GameObject CurrentDot;

    [HideInInspector]
    public float MoveSpeed;
    [HideInInspector]
    public float RotationSpeed;
    [SerializeField]
    Animator bounceAnim;
    [SerializeField]
    AudioClip interactionSXF;
    [SerializeField]
    private bool reactToGravity;
    //[SerializeField]
    //float soundVolume = 1;
    [SerializeField]
    bool isSticky, destroyOnInteract;
    [SerializeField]
    bool floorOnInteract;
    [SerializeField]
    bool ignoreCatapultCollision;

    [SerializeField]
    GameObject interactionParticle;

    private void Start()
    {
        if ((Moves || Rotates) && lilDot != null)
        {
            CurrentDot = Instantiate(lilDot, transform);
            CurrentDot.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y);
            CurrentDot.SetActive(false);
        }
        if (reactToGravity)
        {
            GameManager.Instance.GravitySwitch?.AddListener(Flip);
        }
    }

    public void EnterCollisionWithObject(Rigidbody2D obj)
    {
        switch (Type)
        {
            case ObjectType.NONE:
                break;
            case ObjectType.BOUNCE:
                obj.velocity *= LockDir;
                obj.AddForce(launchStrength * transform.up);
                if (bounceAnim != null)
                {
                    bounceAnim.SetBool("Bounce", true);
                }
                //obj.velocity = launchStrength * transform.up;
                break;
            case ObjectType.STICKY:
                var player = GloopMain.Instance.MyMovement.MyBase;
                player.StickToSurface(transform);
                if (Moves || Rotates)
                {
                    GloopMain.Instance.Rotation.UnrotateCursor = true;
                    player.UnstickToSurfaceEvent.AddListener(MomentumLaunchPlayer);
                    Vector2 rot = transform.position - player.transform.position;
                    GloopMain.Instance.Rotation.RotatePlayerToRotation(Quaternion.AngleAxis((Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg) + 90, Vector3.forward));
                    GloopMain.Instance.Rotation.disableOtherRotations = true;
                }
                break;
            case ObjectType.HURT:
                GloopMain.Instance.RespawnPlayer();
                break;
            case ObjectType.COLLECTABLE:
                Backpack.Instance.AddStar(gameObject);
                gameObject.SetActive(false);
                //TODO: Collect object
                break;
            case ObjectType.CATAPULT:
                var gloop = GloopMain.Instance.MyMovement.MyBase;
                gloop.transform.position = transform.position;
                gloop.StickToSurface(transform);
                gloop.UnstickToSurfaceEvent.AddListener(CatapultPlayer);
                if (Moves || Rotates)
                {
                    GloopMain.Instance.Rotation.UnrotateCursor = true;
                }
                break;
            case ObjectType.BOUNCEPLANT:
                obj.velocity *= LockDir;
                obj.AddForce(launchStrength * transform.up);
                if (bounceAnim != null)
                {
                    bounceAnim.SetBool("Bounce", true);
                }
                PiranhiaPlant tmp = GetComponent<PiranhiaPlant>();
                tmp.Hide();
                break;
            default:
                break;
        }
        if (Moves || Rotates)
        {
            GloopMain.Instance.Rotation.UnrotateCursor = true;
            GloopMain.Instance.MyMovement.MyBase.ParentToPlatform(transform);
        }
        if (floorOnInteract)
        {
            GloopMain.Instance.MyMovement.MyBase.GroundEnter();
        }
        if (!ignoreCatapultCollision && GloopMain.Instance.MyMovement.MyBase.HoldingVelocity == true)
        {
            GloopMain.Instance.MyMovement.MyBase.HoldingVelocity = false;
            GloopMain.Instance.MyMovement.MyBase.InputLocked = false;
        }
        if (interactionParticle != null)
        {
            Instantiate(interactionParticle, transform.position, Quaternion.identity);
        }
        if (interactionSXF != null)
        {
            //SoundManager.Instance.PlayEffect(interactionSXF, soundVolume);
            SoundManager.Instance.PlayEffect(interactionSXF);
            //AudioSource.PlayClipAtPoint(interactionSXF, transform.position);
        }
        if (destroyOnInteract)
        {
            if (floorOnInteract)
            {
                GloopMain.Instance.MyMovement.MyBase.GroundExit();
            }
            if (Moves)
            {
                GloopMain.Instance.MyMovement.MyBase.UnparentFromPlatform();
            }
            Destroy(gameObject);
        }
    }

    public Rigidbody2D AttachToLilDot(Vector2 position)
    {
        CurrentDot.transform.position = position;
        CurrentDot.SetActive(true);
        return CurrentDot.GetComponent<Rigidbody2D>();
    }

    public TonguePoint CreateTonguePoint(Vector2 position)
    {
        //if (Moves || Rotates)
        //{
        CurrentDot.transform.position = position;
        CurrentDot.SetActive(true);
        return new TonguePoint(CurrentDot.GetComponent<Rigidbody2D>(), CurrentDot.transform);
        //}
        //return new TonguePoint(position);
    }

    private void MomentumLaunchPlayer()
    {
        if (Moves)
        {
            GloopMain.Instance.MyMovement.MyBase.Jump();
        }
        if (Rotates)
        {
            Vector2 launchAngle = GloopMain.Instance.MyMovement.transform.position - transform.position;
            var rot = Quaternion.AngleAxis((RotationSpeed / RotationSpeed) * 90, Vector3.forward);
            launchAngle = rot * launchAngle;
            GloopMain.Instance.MyMovement.MyBase.rb.AddForce(launchAngle * launchStrength);
            GloopMain.Instance.Rotation.UnrotateCursor = false;
        }
        GloopMain.Instance.Rotation.disableOtherRotations = false;
        GloopMain.Instance.Rotation.RotateToGravity();
        GloopMain.Instance.MyMovement.MyBase.UnstickToSurfaceEvent.RemoveListener(MomentumLaunchPlayer);
    }

    private void CatapultPlayer()
    {
        GloopMain.Instance.MyMovement.MyBase.VelocityToHold = transform.up.normalized * launchStrength;
        GloopMain.Instance.MyMovement.MyBase.HoldingVelocity = true;
        GloopMain.Instance.MyMovement.MyBase.InputLocked = true;
        //GloopMain.Instance.MyMovement.MyBase.rb.gravityScale = 0;
        GloopMain.Instance.MyMovement.MyBase.UnstickToSurfaceEvent.RemoveListener(CatapultPlayer);
    }

    private void Flip()
    {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {

            GetComponent<Rigidbody2D>().gravityScale *= -1;
            transform.eulerAngles += new Vector3(0, 0, 180);
        }
        else
        {
            Debug.LogWarning($"Object {gameObject.name} wants to flip, but has no rigidbody attached!");
        }
    }

    public void KillLilDot()
    {
        CurrentDot.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EnterCollisionWithObject(GloopMain.Instance.MyMovement.MyBase.rb);
        }
        if (collision.gameObject.tag == "Hookshot")
        {
            GrappleShot gs = collision.gameObject.GetComponent<GrappleShot>();
            gs.AttachToObject(CreateTonguePoint(collision.transform.position));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EnterCollisionWithObject(GloopMain.Instance.MyMovement.MyBase.rb);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (floorOnInteract)
            {
                GloopMain.Instance.MyMovement.MyBase.GroundExit();
            }
            if (collision.transform.parent = transform)
            {
                GloopMain.Instance.MyMovement.MyBase.UnparentFromPlatform();
            }
        }
    }
}

public enum ObjectType
{
    NONE,
    BOUNCE,
    STICKY,
    HURT,
    COLLECTABLE,
    CATAPULT,
    BOUNCEPLANT
}

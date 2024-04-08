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
    Vector2 LockDir = new Vector2(1,1);
    [SerializeField]
    GameObject lilDot;
    public GameObject CurrentDot;
    [HideInInspector, Header("Moving")]
    public Vector3 MaxPos;
    public Vector3 MinPos;
    public float MoveSpeed;
    [Header("Rotation")]
    public Vector3 RotationSpeed;
    [SerializeField]
    Animator bounceAnim;
    [SerializeField]
    AudioClip interactionSXF;
    [SerializeField]
    private bool reactToGravity;
    //[SerializeField]
    //float soundVolume = 1;

    [SerializeField]
    GameObject interactionParticle;

    private void Start()
    {
        if (Moves || Rotates)
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
            case ObjectType.LAUNCH:
                obj.velocity *= LockDir;
                obj.AddForce(launchStrength * transform.up);
                if (bounceAnim != null)
                {
                    bounceAnim.SetBool("Bounce", true);
                }
                //obj.velocity = launchStrength * transform.up;
                break;
            case ObjectType.STICKY:
                break;
            case ObjectType.HURT:
                GloopMain.Instance.RespawnPlayer();
                break;
            case ObjectType.COLLECTABLE:
                Backpack.Instance.AddStar(gameObject);
                gameObject.SetActive(false);
                //TODO: Collect object
                break;
            default:
                break;
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
    }

    public Rigidbody2D AttachToLilDot(Vector2 position)
    {
        CurrentDot.transform.position = position;
        CurrentDot.SetActive(true);
        return CurrentDot.GetComponent<Rigidbody2D>();
    }

    public TonguePoint CreateTonguePoint(Vector2 position)
    {
        if (Moves || Rotates)
        {
            CurrentDot.transform.position = position;
            CurrentDot.SetActive(true);
            return new TonguePoint(CurrentDot.GetComponent<Rigidbody2D>(), CurrentDot.transform);
        }
        return new TonguePoint(position);
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
            gs.AttachToObject(CreateTonguePoint(transform.position));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EnterCollisionWithObject(GloopMain.Instance.MyMovement.MyBase.rb);
        }
    }
}

public enum ObjectType
{
    NONE,
    LAUNCH,
    STICKY,
    HURT,
    COLLECTABLE
}

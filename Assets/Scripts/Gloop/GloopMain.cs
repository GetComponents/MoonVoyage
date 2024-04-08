using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloopMain : MonoBehaviour
{
    public static GloopMain Instance;
    public GloopMove MyMovement;
    [SerializeField]
    GloopFly flyMode;
    [SerializeField]
    GoopWalk walkMode;
    [SerializeField]
    GoopGrapple grappleMode;
    //public Spawnpoint LastCheckpoint;
    public float lookSensitivity;
    [SerializeField]
    float aimRadius;
    public Transform firePoint;
    Vector2 lookPoint;
    //public List<GameObject> LosableObjects;

    [SerializeField]
    private AudioClip respawnSound;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    Animator anim;

    [SerializeField]
    SpriteRenderer sr;

    public UnityEngine.Events.UnityEvent Respawn;
    //public CinemachineVirtualCamera Cinemachine;

    public EMode CurrentMode
    {
        get => currentMode;
        set
        {
            MyMovement.RemoveMode();
            currentMode = value;
            switch (currentMode)
            {
                case EMode.NONE:
                    break;
                case EMode.DEFAULT:
                    break;
                case EMode.FLY:
                    MyMovement = flyMode;
                    break;
                case EMode.WALK:
                    MyMovement = walkMode;
                    break;
                case EMode.GRAPPLE:
                    MyMovement = grappleMode;
                    break;
                default:
                    break;
            }
            MyMovement.AddMode();
        }
    }
    private EMode currentMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        lookSensitivity = PlayerPrefs.GetFloat("MouseSpeed");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MyMovement.AddMode();
        lookPoint.x = 1;
        lookPoint.y = 1;
        GameManager.Instance.LockInput.AddListener(() =>
        {
            if (GameManager.Instance.CursorUnlockers > 0)
            {
                LockInput(true);
            }
            else
            {
                LockInput(false);
            };
        });
        //StartCoroutine(SpawnPlayer());
    }

    //IEnumerator SpawnPlayer()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    RespawnPlayer();
    //}

    void Update()
    {
        MyMovement.MyUpdate();
        FlipCharacter();
        DirectionalInput();
    }

    private void DirectionalInput()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        lookPoint.x += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;

        lookPoint.y += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        lookPoint.x = Mathf.Clamp(lookPoint.x, -1, 1);
        lookPoint.y = Mathf.Clamp(lookPoint.y, -1, 1);
        //Debug.Log(lookPoint);
        if (MyMovement == flyMode)
        {
            firePoint.localPosition = Vector3.zero;
        }
        else
        {
            firePoint.localPosition = Vector3.Normalize(lookPoint) * aimRadius;
        }
    }


    public void RespawnPlayer()
    {
        Respawn?.Invoke();
        /////////////CheckpointEvent/////////////////////////////////////////////////////////
        //if (LastCheckpoint == null)
        //    return;
        //foreach (GameObject gObject in LosableObjects)
        //{
        //    if (gObject != null)
        //    {
        //        gObject.SetActive(true);
        //    }
        //}
        MyMovement.MyBase.GroundedAmount = 0;
        //foreach (GameObject gameObject in LosableObjects)
        //{
        //    if (gameObject.TryGetComponent<ObjectProperty>(out ObjectProperty tmp) && tmp.Type == ObjectType.COLLECTABLE)
        //    {
        //        GameManager.Instance.RemoveStar();
        //    }
        //}
        anim.SetBool("Respawn", true);
        SoundManager.Instance.PlayEffect(respawnSound);
        //LosableObjects = new List<GameObject>();
        //LastCheckpoint.RespawnPlayer();
        rb.velocity = Vector2.zero;
        StartCoroutine(TurnAnimOff());
    }

    private IEnumerator TurnAnimOff()
    {
        yield return new WaitForEndOfFrame();
        anim.SetBool("Respawn", false);
    }

    //public void AddStar(GameObject star)
    //{
    //    LosableObjects.Add(star);
    //    GameManager.Instance.AddStar();
    //}

    //public void SaveProgress()
    //{
    //    int collectedStars = 0;
    //    foreach (GameObject gameObject in LosableObjects)
    //    {
    //        if (gameObject.TryGetComponent<ObjectProperty>(out ObjectProperty tmp) && tmp.Type == ObjectType.COLLECTABLE)
    //        {
    //            collectedStars++;
    //        }
    //    }
    //    //Backpack.Instance.AddStar(collectedStars);
    //    //TODO: Actually save progress
    //    LosableObjects = new List<GameObject>();
    //}

    private void FlipCharacter()
    {
        if (rb.velocity.x >= 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    public void LockInput(bool lockInput)
    {
        MyMovement.MyBase.InputLocked = lockInput;
    }
}

public enum EMode
{
    NONE,
    DEFAULT,
    FLY,
    WALK,
    GRAPPLE
}

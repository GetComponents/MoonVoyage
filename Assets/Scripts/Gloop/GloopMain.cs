using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GloopMain : MonoBehaviour
{
    public static GloopMain Instance;
    public GloopMove MyMovement;
    [SerializeField]
    GloopGlide glideMode;
    [SerializeField]
    GloopDash dashMode;
    [SerializeField]
    GoopGrapple grappleMode;
    [SerializeField]
    GloopGravity gravityMode;
    [SerializeField]
    GloopNone gloopDefault;
    public AnimMethods Rotation;
    //public Spawnpoint LastCheckpoint;
    public float lookSensitivity;
    [SerializeField]
    float aimRadius;
    public Transform AimAnchor;
    public Transform firePoint;
    Vector2 lookPoint;
    private Vector2 lastInput;

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
                    gloopDefault.enabled = true;
                    MyMovement = gloopDefault;
                    break;
                case EMode.GLIDE:
                    glideMode.enabled = true;
                    MyMovement = glideMode;
                    break;
                case EMode.DASH:
                    dashMode.enabled = true;
                    MyMovement = dashMode;
                    break;
                case EMode.GRAPPLE:
                    grappleMode.enabled = true;
                    MyMovement = grappleMode;
                    break;
                case EMode.GRAVITY:
                    gravityMode.enabled = true;
                    MyMovement = gravityMode;
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
        dashMode.enabled = false;
        glideMode.enabled = false;
        grappleMode.enabled = false;
        gravityMode.enabled = false;
        gloopDefault.enabled = false;
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
        MoveCursor();
        //DirectionalInput();
    }

    public void TriggerAbilty(InputAction.CallbackContext context)
    {
        MyMovement.TriggerAbility(context);
    }

    private void MoveCursor()
    {
        if (lastInput == Vector2.zero)
            return;
        lookPoint.x = Mathf.Clamp(lookPoint.x, -1f, 1f);
        lookPoint.y = Mathf.Clamp(lookPoint.y, -1f, 1f);
        //lookPoint.Normalize();
        //lookPoint = Vector2.zero;
        lookPoint.x += (lastInput.x - lookPoint.x) * lookSensitivity * Time.deltaTime;

        lookPoint.y += (lastInput.y - lookPoint.y) * lookSensitivity * Time.deltaTime;
        if (lookPoint == Vector2.zero)
        {
            return;
        }
        //Debug.Log(lookPoint.ToString("0.0000"));


        //lookPoint.x += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;

        //lookPoint.y += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
        Debug.Log(lastInput);
        firePoint.localPosition = lookPoint.normalized * aimRadius;
    }

    public void MoveMouse(InputAction.CallbackContext context)
    {
        ////return;
        ////Cursor.lockState = CursorLockMode.Locked;
        //Vector2 vec = context.ReadValue<Vector2>();
        //if (vec == Vector2.zero)
        //    return;
        //lastInput = vec.normalized;

        ////lookPoint.x += vec.x * lookSensitivity * Time.deltaTime;

        ////lookPoint.y += vec.y * lookSensitivity * Time.deltaTime;


        //////lookPoint.x += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;

        //////lookPoint.y += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        ////lookPoint.x = Mathf.Clamp(lookPoint.x, -1, 1);
        ////lookPoint.y = Mathf.Clamp(lookPoint.y, -1, 1);
        ////firePoint.localPosition = Vector3.Normalize(lookPoint) * aimRadius;
    }


    public void MoveController(InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();
        if (vec == Vector2.zero)
            return;
        lastInput = vec.normalized;
        ////Debug.Log(vec);
        //lookPoint.x += vec.x * lookSensitivity * Time.deltaTime;
        //lookPoint.y += vec.y * lookSensitivity * Time.deltaTime;
        //lookPoint.x = Mathf.Clamp(lookPoint.x, -1, 1);
        //lookPoint.y = Mathf.Clamp(lookPoint.y, -1, 1);
        ////vec.x = Mathf.Clamp(vec.x, -1, 1);
        ////vec.y = Mathf.Clamp(vec.y, -1, 1);
        //firePoint.localPosition = Vector3.Normalize(lookPoint) * aimRadius;
        //Debug.Log("Left: " + lookPoint);
    }


    public void RespawnPlayer()
    {
        Respawn?.Invoke();
        Backpack.Instance.RespawnPlayer();
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
        if (rb.velocity.x * MyMovement.MyBase.rb.gravityScale >= 0)
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
    GLIDE,
    DASH,
    GRAPPLE,
    GRAVITY
}

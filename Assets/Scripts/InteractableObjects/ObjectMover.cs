using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    public float RotationSpeed, MovementSpeed;
    private Rigidbody2D rb;
    [SerializeField]
    List<Transform> Path;
    Vector2 previousDestination, nextDestination;
    [SerializeField]
    float minRotation, maxRotation;
    [SerializeField]
    bool goFullCircle;
    int pathIndex;
    float destinationCompletion;
    public Vector2 ProjectileDirection;
    [SerializeField]
    EMovementType movement;
    [SerializeField]
    float StopTimer;
    float currentTimer;
    [SerializeField]
    GameObject dot;

    private void Awake()
    {
        if (TryGetComponent<ObjectProperty>(out ObjectProperty op))
        {
            op.RotationSpeed = RotationSpeed;
            if (RotationSpeed != 0)
            {
                op.Rotates = true;
            }
            op.MoveSpeed = MovementSpeed;
            if (MovementSpeed != 0)
            {
                op.Moves = true;
            }
        }
        if (movement == EMovementType.PROJECTILE)
        {
            rb = GetComponent<Rigidbody2D>();
            return;
        }
        if (MovementSpeed > 0)
        {
            NextPoint();
        }
    }

    void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        if (RotationSpeed == 0)
        {
            return;
        }
        if (movement == EMovementType.NONE)
        {
            transform.eulerAngles += new Vector3(0, 0, RotationSpeed * Time.deltaTime);
            return;
        }



        if (movement == EMovementType.STOPAndGO && currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            return;
        }
        destinationCompletion += Time.deltaTime * RotationSpeed;
        if (destinationCompletion >= 1)
        {
            ChangeRotation();
        }
        float currentCompletion = 0;
        switch (movement)
        {
            case EMovementType.NONE:
                Debug.LogWarning("No Movementtype defined in Object" + gameObject.name);
                break;
            case EMovementType.LINEAR:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.SINE:
                currentCompletion = (Mathf.Sin((destinationCompletion * Mathf.PI) - 0.5f) / 2) + 0.5f;
                break;
            case EMovementType.STOPAndGO:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.PROJECTILE:
                rb.velocity = ProjectileDirection * MovementSpeed;
                return;
            default:
                break;

        }
        //transform.eulerAngles = Vector3.Lerp(previousDestination, nextDestination, currentCompletion);
        Vector3 tmp = Vector3.zero;
        tmp.z = Mathf.LerpAngle(minRotation, maxRotation, currentCompletion);
        transform.eulerAngles = tmp;
    }

    private void ChangeRotation()
    {
        float tmp = minRotation;
        minRotation = maxRotation;
        maxRotation = tmp;
        destinationCompletion = 0;
        currentTimer = StopTimer;
    }

    private void Move()
    {
        if (MovementSpeed == 0)
            return;

        if (movement == EMovementType.STOPAndGO && currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            return;
        }
        if (movement != EMovementType.PROJECTILE)
            destinationCompletion += Time.deltaTime * MovementSpeed;
        if (destinationCompletion >= 1)
        {
            NextPoint();
        }
        float currentCompletion = 0;
        switch (movement)
        {
            case EMovementType.NONE:
                Debug.LogWarning("No Movementtype defined in Object" + gameObject.name);
                break;
            case EMovementType.LINEAR:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.SINE:
                currentCompletion = (Mathf.Sin((destinationCompletion * Mathf.PI) - 0.5f) / 2) + 0.5f;
                break;
            case EMovementType.STOPAndGO:
                currentCompletion = destinationCompletion;
                break;
            case EMovementType.PROJECTILE:
                rb.velocity = ProjectileDirection * MovementSpeed;
                return;
            default:
                break;

        }
        transform.position = Vector3.Lerp(previousDestination, nextDestination, currentCompletion);
    }

    private void NextPoint()
    {
        previousDestination = Path[pathIndex].position;
        pathIndex++;
        if (pathIndex >= Path.Count)
        {
            pathIndex = 0;
        }
        nextDestination = Path[pathIndex].position;
        destinationCompletion = 0;
        currentTimer = StopTimer;
    }
}

public enum EMovementType
{
    NONE,
    LINEAR,
    SINE,
    STOPAndGO,
    PROJECTILE
}

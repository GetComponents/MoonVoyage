using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed, movementSpeed;

    [SerializeField]
    List<Transform> Path;
    Vector2 previousDestination, nextDestination;
    int pathIndex;
    float destinationCompletion;

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
            op.RotationSpeed = rotationSpeed;
            if (rotationSpeed != 0)
            {
                op.Rotates = true;
            }
            op.MoveSpeed = movementSpeed;
            if (movementSpeed != 0)
            {
                op.Moves = true;
            }
        }
        if (movementSpeed > 0)
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
        if (rotationSpeed != 0)
        {
            transform.eulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        if (movementSpeed == 0)
            return;

        if (movement == EMovementType.STOPAndGO && currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            return;
        }
        destinationCompletion += Time.deltaTime * movementSpeed;
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
    STOPAndGO
}

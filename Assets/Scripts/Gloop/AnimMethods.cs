using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMethods : MonoBehaviour
{
    //[SerializeField]
    public Rigidbody2D m_rigidbody2D;
    [SerializeField]
    Transform Cursor;

    [SerializeField]
    float timeToReturnRot;
    public bool Rotate
    {
        get => m_rotate;
        set
        {
            m_rotate = value;
            if (m_rotate == false)
            {
                RotateToGravity();
            }
        }
    }
    private bool m_rotate;
    public bool UnrotateCursor
    {
        get => m_unrotateCursor;
        set
        {
            m_unrotateCursor = value;
            if (m_unrotateCursor == false)
            {
                Cursor.localEulerAngles = Vector3.zero;
            }
        }
    }
    private bool m_unrotateCursor;
    public float Gravity;
    public bool disableOtherRotations;

    private void Start()
    {
        Gravity = m_rigidbody2D.gravityScale;
    }

    private void Update()
    {
        if (UnrotateCursor)
        {
            Cursor.eulerAngles = Vector3.zero;
        }
        if (Rotate)
        {
            RotateToVelocity();
            //transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg, Vector3.forward);
        }
    }

    public void RotatePlayerToRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void RotatePlayerToRotation(Vector3 rotation)
    {
        transform.eulerAngles = rotation;
    }

    public void ChangeGravity()
    {
        Gravity *= -1;
        RotateToGravity();
    }

    public void RotateToGravity()
    {
        if (!disableOtherRotations)
        {
            transform.eulerAngles = new Vector3(0, 0, 90 - (90 * Gravity));
        }
    }

    public void RotateToVelocity()
    {
        if (!disableOtherRotations)
        {
            Vector2 v = m_rigidbody2D.velocity;
            transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg) - 90, Vector3.forward);
        }
    }

    //public void UnrotateCharacter()
    //{
    //}
}

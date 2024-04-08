using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GloopCollision : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    //public UnityEvent OnCollision;
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //Debug.Log("I collided");
    //    if (collision.transform.TryGetComponent(out ObjectProperty op))
    //    {
    //        OnCollision.Invoke();
    //        op.EnterCollisionWithObject(rb);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //Debug.Log("I collided");
    //    if (collision.transform.TryGetComponent(out ObjectProperty op))
    //    {
    //        OnCollision.Invoke();
    //        op.EnterCollisionWithObject(rb);
    //    }
    //}
}

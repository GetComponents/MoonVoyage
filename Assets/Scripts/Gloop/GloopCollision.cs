using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GloopCollision : MonoBehaviour
{
    [SerializeField]
    GloopMoveBase gloopMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"I, {gameObject.name} collided with {collision.gameObject.name}");
        if (collision.tag == "Floor")
        {
            gloopMove.GroundEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            gloopMove.GroundExit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMethods : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D m_rigidbody2D;

    [SerializeField]
    float timeToReturnRot;
    public bool rotate;

    private void Update()
    {
        if (rotate)
        {
            Vector2 v = m_rigidbody2D.velocity;
            transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg) - 90, Vector3.forward);
            //transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg, Vector3.forward);

        }
    }


    public void RotateCharacter()
    {
        rotate = true;
        StartCoroutine(ReturnToDefault());
    }

    private IEnumerator ReturnToDefault()
    {
        yield return new WaitForSeconds(timeToReturnRot);
        transform.eulerAngles = new Vector3(0,0, 90 - (90 * m_rigidbody2D.gravityScale));
        rotate = false;
    }

    //public void UnrotateCharacter()
    //{
    //}
}

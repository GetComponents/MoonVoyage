using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraLookahead : MonoBehaviour
{
    [SerializeField]
    private float LookaheadTime;
    private bool actionCancel = true;
    private Vector2 lookValue;
    float defaultValue;
    [SerializeField]
    float LookaheadValue;
    [SerializeField]
    float minLookValue;
    [SerializeField]
    private CinemachineVirtualCamera cineCam;
    private Coroutine currentTimer;

    private void Start()
    {
        defaultValue = cineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY;
    }

    public void PlayerInput(InputAction.CallbackContext context)
    {
        lookValue = context.ReadValue<Vector2>();
        float minValue = GloopMain.Instance.MyMovement.MyBase.MinMovement;
        if ((Mathf.Abs(lookValue.y) > minLookValue || Mathf.Abs(lookValue.x) > minLookValue)
            && actionCancel == true)
        {
            actionCancel = false;
            //Debug.Log("Look");
            if (currentTimer != null)
                StopCoroutine(currentTimer);
            currentTimer = StartCoroutine(LookaheadTimer());
        }
        else if ((Mathf.Abs(lookValue.y) < minLookValue && Mathf.Abs(lookValue.x) < minLookValue)
             && actionCancel == false)
        {
            //Debug.Log("Cancel, " + lookValue);
            CinemachineFramingTransposer tmp = cineCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            tmp.m_ScreenY = defaultValue;
            tmp.m_ScreenX = defaultValue;
            actionCancel = true;
        }
    }

    private IEnumerator LookaheadTimer()
    {
        yield return new WaitForSeconds(LookaheadTime);
        if (actionCancel == false && GloopMain.Instance.MyMovement.MyBase.GroundedAmount > 0)
            Lookahead();
    }

    private void Lookahead()
    {
        CinemachineFramingTransposer tmp = cineCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        //defaultValue = tmp.m_ScreenY;
        //defaultValue = tmp.m_ScreenX;

        if (Mathf.Abs(lookValue.y) > Mathf.Abs(lookValue.x))
        {
            tmp.m_ScreenY += lookValue.y * LookaheadValue;
        }
        else
        {
            tmp.m_ScreenX -= lookValue.x * LookaheadValue;
        }
    }
}

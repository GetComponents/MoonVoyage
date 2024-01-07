using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraSettings : MonoBehaviour
{
    [SerializeField]
    float lenseSize;
    [SerializeField]
    float PlayerYPos, yDamping, yDeadzone;
    bool lerpSettings;
    [SerializeField]
    float maxLerpTime;
    //float lerpTime;
    //[SerializeField]
    //CinemachineFramingTransposer tp;

    CinemachineVirtualCamera cm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ChangeCameraSetting();
        }
    }

    private void ChangeCameraSetting()
    {
        cm = GloopMain.Instance.Cinemachine;
        StartCoroutine(LerpSettings(cm.m_Lens.OrthographicSize, cm.m_Lens.LensShift.y));
    }

    private IEnumerator LerpSettings(float previousLenseSize, float previousPlayerPos)
    {
        Debug.Log("AM lerping");
        CinemachineFramingTransposer tmp = cm.GetCinemachineComponent<CinemachineFramingTransposer>();
        tmp.m_DeadZoneHeight = yDeadzone;
        tmp.m_YDamping = yDamping;
        for (float i = 0; i < maxLerpTime; i += Time.deltaTime)
        {
            cm.m_Lens.OrthographicSize = Mathf.Lerp(previousLenseSize, lenseSize, i / maxLerpTime);
            tmp.m_ScreenY = Mathf.Lerp(previousPlayerPos, PlayerYPos, i / maxLerpTime);
            yield return new WaitForSeconds(0);
            //tmp.m_YDamping =
            //tmp = tp;
        }
        tmp.m_ScreenY = PlayerYPos;
        cm.m_Lens.OrthographicSize = lenseSize;
        Destroy(gameObject);
    }
}

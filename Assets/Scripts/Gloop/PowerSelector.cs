using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSelector : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EnableCanvas(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            EnableCanvas(false);
        }
    }

    public void ChangeState(int stateIndex)
    {
        EMode mode = EMode.NONE;
        switch (stateIndex)
        {
            case 0:
                break;
            case 1:
                mode = EMode.DASH;
                break;
            case 2:
                mode = EMode.GLIDE;
                break;
            case 3:
                mode = EMode.GRAPPLE;
                break;
            case 4:
                mode = EMode.GRAVITY;
                break;
            default:
                break;
        }
        GloopMain.Instance.CurrentMode = mode;
    }

    public void EnableCanvas(bool enable)
    {
        canvas.enabled = enable;

        if (canvas.enabled)
        {
            GameManager.Instance.CursorUnlockers++;
            GameManager.Instance.TimeScalers++;
        }
        else
        {
            GameManager.Instance.CursorUnlockers--;
            GameManager.Instance.TimeScalers--;
        }
    }
}

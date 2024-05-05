using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerSelector : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;

    public void OpenSelector(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EnableCanvas(true);
        }
        else if (context.canceled)
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

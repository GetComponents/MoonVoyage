using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerSelectorUI : MonoBehaviour
{
    [SerializeField]
    Image topImage, rightImage, bottomImage, leftImage;
    [SerializeField]
    Color defaultColor, topColor, rightColor, bottomColor, leftColor;
    [SerializeField]
    Canvas canvas;


    Image lastImage;
    Vector2 lastInput;

    private void Start()
    {
        lastImage = topImage;
        topImage.color = defaultColor;
        rightImage.color = defaultColor;
        bottomImage.color = defaultColor;
        leftImage.color = defaultColor;
    }

    public void OpenSelector(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EnableCanvas(true);
        }
        else if (context.canceled)
        {
            EnableCanvas(false);
            ChangeState(lastInput);
        }
    }

    public void ChangeState(Vector2 dir)
    {
        int stateIndex = 0;
        if (dir.x > 0.5f)
        {
            stateIndex = 2;
        }
        else if (dir.x < -0.5f)
        {
            stateIndex = 4;
        }
        else if (dir.y > 0.5f)
        {
            stateIndex = 1;
        }
        else if (dir.y < -0.5f)
        {
            stateIndex = 3;
        }
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
                mode = EMode.GRAVITY;
                break;
            case 4:
                mode = EMode.GRAPPLE;
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

    public void SelectPower(InputAction.CallbackContext context)
    {
        if (context.canceled)
            return;
        Vector2 dir = context.ReadValue<Vector2>().normalized;
        lastInput = dir;
        if (dir.x > 0.5f)
        {
            Highlight(rightImage, rightColor);
        }
        else if (dir.x < -0.5f)
        {
            Highlight(leftImage, leftColor);
        }
        else if (dir.y > 0.5f)
        {
            Highlight(topImage, topColor);
        }
        else if (dir.y < -0.5f)
        {
            Highlight(bottomImage, bottomColor);
        }
    }

    private void Highlight(Image image, Color hColor)
    {
        lastImage.color = defaultColor;
        lastImage = image;
        lastImage.color = hColor;
    }
}

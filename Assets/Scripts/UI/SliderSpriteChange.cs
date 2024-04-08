using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSpriteChange : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    Image knob;

    [SerializeField]
    Sprite[] allSprites;

    public void ValueChange()
    {
        if (slider.value != 0)
        {
            knob.sprite = allSprites[Mathf.CeilToInt(slider.value * allSprites.Length) - 1];
        }
        else
        {
            knob.sprite = allSprites[0];
        }
    }
}

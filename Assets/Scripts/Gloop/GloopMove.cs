using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GloopMove : MonoBehaviour
{
    public Color ModeColor;
    public SpriteRenderer ModeSprite;
    public bool InputLocked;
    //[SerializeField]
    public Animator GloopAnim;
    public AudioSource MySoundtrack;
    public float SoundtrackVolume;
    public int groundedAmount
    {
        get => m_groundedAmount;
        set
        {
            if (value < 0)
            {
                m_groundedAmount = 0;
            }
            else
            {
                m_groundedAmount = value;
            }
        }
    }
    int m_groundedAmount;

    public abstract void MyUpdate();

    public abstract void RemoveMode();

    public abstract void AddMode();
}

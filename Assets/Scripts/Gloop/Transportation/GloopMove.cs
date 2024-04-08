using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GloopMove : MonoBehaviour
{
    public GloopMoveBase MyBase;
    public Color ModeColor;
    public SpriteRenderer ModeSprite;
    //[SerializeField]
    public AudioSource MySoundtrack;
    public float SoundtrackVolume;

    public abstract void MyUpdate();
    

    public abstract void RemoveMode();

    public abstract void AddMode();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class GloopMove : MonoBehaviour
{
    public GloopMoveBase MyBase;
    public Color ModeColor;
    public SpriteRenderer ModeSprite;
    //[SerializeField]
    public AudioSource MySoundtrack;
    public float SoundtrackVolume;

    public abstract void TriggerAbility(InputAction.CallbackContext context);

    public abstract void MyUpdate();

    public abstract void EnterGround(); 
    public abstract void ExitGround(); 

    public abstract void RemoveMode();

    public abstract void AddMode();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundtrack : MonoBehaviour
{
    [SerializeField]
    bool startSoundtrack;

    private void Start()
    {
        SoundManager.Instance.PlaySoundtrack = startSoundtrack;
    }
}

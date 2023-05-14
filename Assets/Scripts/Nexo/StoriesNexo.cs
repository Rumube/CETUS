using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoriesNexo : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter _sound;

    public void PlaySound()
    {
        _sound.Play();
    }
   
}

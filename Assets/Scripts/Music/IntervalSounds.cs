using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class IntervalSounds : MonoBehaviour
{  
    [Serializable]
    public struct Sound{
        public string name;
        public StudioEventEmitter soundsWithRandomInterval;
        [Range(10.0f, 100.0f)] public float minimumTimeToSound;
        private float _lastTimeExecuted;
    }
    [SerializeField] private List<Sound> _sounds;
   
    void Start()
    {
        
    }

    IEnumerator PlaySounds()
    {
        while (true)
        {
            foreach (Sound sound in _sounds)
            {
                
            }
        }
    }
}

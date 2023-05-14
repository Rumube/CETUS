using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class InitialMusic : MonoBehaviour
{
    FMOD.Studio.Bus Master;
    [SerializeField] private Compass compass;
    [SerializeField] private StudioEventEmitter _sound;
    bool completed;
    bool start;
    public bool finishing;
    float counter = 0;
    void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    void Update()
    {
        if (compass._currentMemories == 1 && !start && !finishing)
        {
            start = true;
            _sound.Play();
        }

        if (compass._currentMemories != 8 && !completed && !finishing)
        {
            Master.setVolume((float)compass._currentMemories / 8.0f);
        }
        else
        {
            completed = true;
        }
        if (finishing)
        {
            counter += Time.deltaTime;
            Master.setVolume(1 - (counter / 3.0f));
        }
    }

}

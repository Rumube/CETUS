using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoriesNexo : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private StudioEventEmitter _volcano;
    [SerializeField] private StudioEventEmitter _market;
    [SerializeField] private StudioEventEmitter _kids;

    public void PlayVolcano()
    {
        _volcano.Play();
    }
    public void PlayMarket()
    {
        _market.Play();
    }
    public void PlayKids()
    {
        _kids.Play();
    }
}

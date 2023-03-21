using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class CollectedMemory : SpaceObject
{
    //References
    private StudioEventEmitter _cristalFound;
    private MeshFilter _meshFilter;     
    // Start is called before the first frame update
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _cristalFound = GetComponent<StudioEventEmitter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Whale>().GetCompass().MemoriesUp();
            _cristalFound.Play();
            StartCoroutine(DestroyAfterSound());
            
        }
    }
    IEnumerator DestroyAfterSound()
    {
        yield return new WaitUntil(()=>!_cristalFound.IsPlaying());
        Destroy(gameObject);
    }
}

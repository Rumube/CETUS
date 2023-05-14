using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System.Diagnostics;

public class CollectedMemory : SpaceObject
{
    //References
    private StudioEventEmitter _cristalFound;
    private bool _found;
    private MeshFilter _meshFilter;
    private GameObject _player;
    private bool _used = false;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _scaleSpeed;

    public enum Zone
    {
        zone1 = 0,
        zone3 = 3,
        zone6 = 6
    }

    [SerializeField] private Zone _zone;
    // Start is called before the first frame update
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _cristalFound = GetComponent<StudioEventEmitter>();
        switch (tag)
        {
            case "FragmentsZone1":
                _zone = Zone.zone1;
                break;
            case "FragmentsZone3":
                _zone = Zone.zone3;
                break;
            case "FragmentsZone6":
                _zone = Zone.zone6;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if( _found)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _moveSpeed * Time.deltaTime);
            if(transform.localScale.x >= _scaleSpeed)
            {
                transform.localScale -= new Vector3(_scaleSpeed, _scaleSpeed, _scaleSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !_used)
        {
            _used = true;
            _player = other.gameObject;
            other.gameObject.GetComponent<Whale>().GetCompass().MemoriesUp(_zone);
            _cristalFound.Play();
            _found = true;
            StartCoroutine(DestroyAfterSound());
            
        }
    }
    IEnumerator DestroyAfterSound()
    {
        yield return new WaitUntil(()=>!_cristalFound.IsPlaying());
        Destroy(gameObject);
    }
}

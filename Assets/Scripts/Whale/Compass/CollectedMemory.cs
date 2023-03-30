using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class CollectedMemory : SpaceObject
{
    //References
    private StudioEventEmitter _cristalFound;
    private bool _found;
    private MeshFilter _meshFilter;
    private GameObject _player;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _scaleSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _cristalFound = GetComponent<StudioEventEmitter>();
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
        if (other.tag == "Player")
        {
            _player = other.gameObject;
            other.gameObject.GetComponent<Whale>().GetCompass().MemoriesUp();
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

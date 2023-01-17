using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedMemory : SpaceObject
{
    [Header("Mesh")]
    [SerializeField]
    private MeshFilter _meshFilter;
    // Start is called before the first frame update
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Whale>().GetCompass().MemoriesUp();
            Destroy(gameObject);
        }
    }
}

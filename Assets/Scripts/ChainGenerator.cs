using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainGenerator : MonoBehaviour
{
    [Range(5, 30)] [SerializeField] private int _chainsNumber;
    public float distance;
    public GameObject chain;
    List<GameObject> chains= new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [ExecuteInEditMode]
    // Update is called once per frame
    void Update()
    {
       //while (chains.Count < _chainsNumber)
       // {
       //     GameObject clone = Instantiate(chain, new Vector3(transform.position.x, transform.position.y + distance, transform.position.z), transform.rotation);
       //     chains.Add(clone);
       //     Debug.Log(chains.Count + " chainn " + _chainsNumber);
            
       // }
        if (chains.Count < _chainsNumber)
        {
            for (int i = 0; i < _chainsNumber; i++)
            {
                GameObject clone;
                if (i % 2 == 0)
                {
                    clone = Instantiate(chain, new Vector3(transform.position.x, transform.position.y + i * distance, transform.position.z), transform.rotation);
                }
                else
                {
                    clone = Instantiate(chain, transform.position, Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z));
                }
                chains.Add(clone);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChainGenerator : MonoBehaviour
{
    [Range(0, 30)] [SerializeField] private int _chainsNumber;
    public float distance;
    public GameObject chain;
   public  List<GameObject> chains= new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                    if (chains.Count % 2 == 0)
                    {
                        clone = Instantiate(chain, new Vector3(transform.position.x + chains.Count - 1 * distance, transform.position.y , transform.position.z), transform.rotation);
                    }
                    else
                    {
                        clone = Instantiate(chain, new Vector3(transform.position.x + chains.Count - 1 * distance, transform.position.y , transform.position.z), Quaternion.Euler(90, transform.rotation.y, transform.rotation.z));
                    }
                    chains.Add(clone);
                }
            }
            else if (chains.Count > _chainsNumber)
            {
                DestroyImmediate(chains[chains.Count - 1]);
                chains.Remove(chains[chains.Count - 1]);
            }

    }
}

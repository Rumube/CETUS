using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform center;
    public float speed;
    Vector3 _vectorRandom;
    bool wait;
    // Start is called before the first frame update
    void Start()
    {
        _vectorRandom = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(center.position, _vectorRandom, speed * Time.deltaTime);
        //int random = Random.Range(-1, 2);
        if (wait)
        {
            StartCoroutine(RotationTime());
        }
      
        
    }
   
    IEnumerator RotationTime()
    {
        wait = true;
       
        yield return new WaitForSeconds(10f);
       
        _vectorRandom = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));
        wait = false;
    }
}

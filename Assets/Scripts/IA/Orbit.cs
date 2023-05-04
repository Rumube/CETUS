using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform center;
    public float speed;
    Vector3 _vectorRandom;
    Vector3 lastVector;
    bool wait;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        _vectorRandom = new Vector3(0, Random.Range(-1, 2), Random.Range(-1, 2));
        lastVector = _vectorRandom;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_vectorRandom==Vector3.zero)
        {
            _vectorRandom = Vector3.up;
        }
        transform.RotateAround(center.position, Vector3.Slerp(lastVector, _vectorRandom, Time.deltaTime)+transform.forward, speed * Time.deltaTime);
       //transform.forward = Vector3.Slerp(lastVector, _vectorRandom, Time.deltaTime);
        Debug.Log( startTime+"last " + lastVector+" random "+ _vectorRandom+" lerp "+Vector3.Lerp(lastVector, _vectorRandom, speed * Time.deltaTime));
        //transform.RotateAround(center.position, -Vector3.forward, speed * Time.deltaTime);
        //transform.LookAt(Vector3.forward);
        //int random = Random.Range(-1, 2);
        if (!wait && Vector3.Slerp(lastVector, _vectorRandom, Time.deltaTime) ==_vectorRandom)
        {
            StartCoroutine(RotationTime());
            //transform.position = Vector3.Lerp(lastVector, _vectorRandom, 5);
        }
      
        
    }
   
    IEnumerator RotationTime()
    {
        wait = true;
        
        yield return new WaitForSeconds(5f);
        //speed = Random.Range(1, 360);
        lastVector = _vectorRandom;
        _vectorRandom = new Vector3(0, Random.Range(-1, 2), Random.Range(-1, 2));
        Debug.Log("change ");
        startTime = Time.time;
        wait = false;
    }
}

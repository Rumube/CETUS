using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    //Parameters
    [Header("Parameters")]
    public float _followVelocity;

    private GameObject _target;


    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FollowMovement();
    }

    private void FollowMovement()
    {
        transform.LookAt(_target.transform);
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _followVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Trigger: " + other.name);
        //if(other.tag == "Player")
        //{
        //    other.GetComponent<Whale>().LightDown();
        //    GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        //    camera.GetComponent<CameraShake>().StartShake(0.5f, 1f);
        //    Destroy(gameObject);
        //}
    }
}

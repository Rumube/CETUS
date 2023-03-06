using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private Vector3 _newUp = Vector3.zero;
    private bool _rotating = false;

    private void Awake()
    {
        //_newUp = transform.up;
    }

    private void Update()
    {
        //UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (_rotating)
        {
            transform.up = Vector3.Slerp(transform.up, _newUp, Time.deltaTime * 1f);
            print(transform.up + " == " +  _newUp);
            if(Vector3.Distance(transform.up, _newUp) <= 0.5f)
            {
                _rotating = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //_newUp = collision.contacts[0].normal;
        //_rotating = true;
        transform.up = collision.contacts[0].normal;
    }
}

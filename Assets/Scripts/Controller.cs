using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rb;

    //speed
    public float _turnSpeed = 60;
    public float _boostSpeed = 5f;




    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Turn();
        MoveUpdate();
    }

    private void Turn()
    {
        float yaw = _turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float pitch = _turnSpeed * Time.deltaTime * -Input.GetAxis("Vertical");
        float roll = _turnSpeed * Time.deltaTime * Input.GetAxis("Rotate");
        _transform.Rotate(pitch, yaw, roll);
    }

    private void MoveUpdate()
    {
        _transform.position += _transform.forward * _boostSpeed * Time.deltaTime;
    }

}

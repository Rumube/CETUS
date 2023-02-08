using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField]
    private float _turnSpeed = 60f;
    [SerializeField]
    private float _boostSpeed = 45f;
    private Animator _animator;
    private float _horizontalValue;
    private float _verticalValue;
    private float _rotateValue;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        Inputs();
        Turn();
        Thrust();
        Animation();
    }
    private void Inputs()
    {
        _horizontalValue = Input.GetAxis("Horizontal");
        _verticalValue = Input.GetAxis("Vertical");
        _rotateValue = Input.GetAxis("Rotate");
    }
    private void Turn()
    {
        float yaw = _turnSpeed * Time.fixedDeltaTime * Input.GetAxis("Horizontal");
        float pitch = _turnSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical");
        float roll = _turnSpeed * Time.fixedDeltaTime * Input.GetAxis("Rotate");
        print("Roll -> " + roll);
        transform.Rotate(pitch, yaw, roll);
    }
    private void Thrust()
    {
        transform.position += transform.forward * _boostSpeed * Time.fixedDeltaTime;
    }
    private void Animation()
    {
        _animator.SetBool("Space", Input.GetKey("space"));
        _animator.SetBool("Left", _horizontalValue < 0 ? true : false);
        _animator.SetBool("Right", _horizontalValue > 0 ? true : false);
        _animator.SetBool("Up", _verticalValue > 0 ? true : false);
        _animator.SetBool("Down", _verticalValue < 0 ? true : false);
    }
}

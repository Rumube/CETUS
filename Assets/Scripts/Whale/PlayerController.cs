using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Referenes")]
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputMovement;

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

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Gameplay.Enable();
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
        _inputMovement = _playerInputActions.Gameplay.Movement.ReadValue<Vector2>();
        _horizontalValue = _inputMovement.x;
        _verticalValue = _inputMovement.y;
        _rotateValue = _playerInputActions.Gameplay.Rotate.ReadValue<float>();
    }
    private void Turn()
    {
        float yaw = _turnSpeed * Time.fixedDeltaTime * _horizontalValue;
        float pitch = _turnSpeed * Time.fixedDeltaTime * _verticalValue;
        float roll = _turnSpeed * Time.fixedDeltaTime * _rotateValue;
        transform.Rotate(pitch, yaw, roll);
    }
    private void Thrust()
    {
        transform.position += transform.forward * _boostSpeed * Time.fixedDeltaTime;
    }
    private void Animation()
    {
        //_animator.SetBool("Space", Input.GetKey("space"));
        _animator.SetBool("Left", _horizontalValue < 0 ? true : false);
        _animator.SetBool("Right", _horizontalValue > 0 ? true : false);
        _animator.SetBool("Up", _verticalValue > 0 ? true : false);
        _animator.SetBool("Down", _verticalValue < 0 ? true : false);
    }
}

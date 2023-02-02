using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;

    [Header("Speed")]
    [SerializeField]
    private float _speed = 60;
    [SerializeField]
    private float _turnYawSpeed = 10;
    [SerializeField]
    private float _turnPitchSpeed = 60;
    [SerializeField]
    private float _turnRollSpeed = 20;
    [SerializeField]
    private float _boostSpeed = 5f;
    [SerializeField]
    private float _impulseTime;
    [SerializeField]
    private float _impulseCooldown;

    [Header("Cameras")]
    [SerializeField]
    private GameObject _dashCamera;

    [Header("MaxValues")]
    [SerializeField]
    private float _maxYawSpeed = 60;
    [SerializeField]
    private float _maxPitchSpeed = 100;
    [SerializeField]
    private float _maxRollSpeed = 70;

    [Header("RotationPoints")]
    [SerializeField]
    private Transform rightPoint;
    [SerializeField]
    private Transform leftPoint;

    //Controls
    private float _horizontalValue;
    private float _verticalValue;
    private float _rotateValue;
    private bool _impulse;
    private float _stopImpulse;
    private float _nextImpulse;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();       
    }

    private void Update()
    {
        InputUpdate();
        Turn();
        MoveUpdate();
        Animation();
    }


    private void InputUpdate()
    {
        _horizontalValue = Input.GetAxis("Horizontal");
        _verticalValue = Input.GetAxis("Vertical");
        _rotateValue = Input.GetAxis("Rotate");
        if (Input.GetKey(KeyCode.Space) && !_impulse && Time.realtimeSinceStartup > _nextImpulse)
        {
            _impulse = true;
            _stopImpulse = Time.realtimeSinceStartup + _impulseTime;
            _dashCamera.SetActive(true);
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            ActiveSonar();
        }
    }

    private void Turn()
    {
        float yaw = _turnYawSpeed * Time.deltaTime * _horizontalValue;
        float pitch = _turnPitchSpeed * Time.deltaTime * -_verticalValue;
        float roll = _turnRollSpeed * Time.deltaTime * _rotateValue;

        transform.RotateAround(yaw > 0 ? rightPoint.position:leftPoint.position, Vector3.up, yaw);
        transform.Rotate(pitch, 0,roll);
    }

    private void Animation()
    {
        _animator.SetBool("Space", Input.GetKey("space"));
        _animator.SetBool("Left", _horizontalValue < 0 ? true: false);
        _animator.SetBool("Right", _horizontalValue > 0 ? true : false);
        _animator.SetBool("Up", _verticalValue > 0 ? true : false);
        _animator.SetBool("Down", _verticalValue < 0 ? true : false);
    }

    private void ActiveSonar()
    {
        GetComponent<SonarController>().StartSonar();
    }
   
    private void MoveUpdate()
    {
        float newBoost = _boostSpeed;
        if (_impulse)
        {
            newBoost *= 10;
        }

        if (Time.realtimeSinceStartup >= _stopImpulse && _impulse)
        {
            _impulse = false;
            _nextImpulse = Time.realtimeSinceStartup + _impulseCooldown;
            _dashCamera.SetActive(false);
        }
        transform.position += transform.forward * newBoost * Time.deltaTime;
    }   

    //GETTERS
    //public float GetTurnSpeed()
    //{
    //    return _turnSpeed;
    //}
    public float GetBoostSpeed()
    {
        return _boostSpeed;
    }

    //SETTERS
    public void SetTurnSpeed(float newValue)
    {
        _maxYawSpeed = Mathf.Min(newValue, _maxYawSpeed);
        _maxPitchSpeed = Mathf.Min(newValue, _maxPitchSpeed);
        _maxRollSpeed = Mathf.Min(newValue, _maxRollSpeed);
    }
    public void SetboostSpeed(float newValue)
    {
        _boostSpeed = newValue;
    }
}

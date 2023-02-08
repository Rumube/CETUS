using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    [Header("References")]
    [SerializeField]
    private GameObject _dashCamera;
    [SerializeField]
    private Compass _compass;
    [SerializeField]
    private Transform rightPoint;
    [SerializeField]
    private Transform leftPoint;

    [Header("MaxValues")]
    [SerializeField]
    private float _maxYawSpeed = 60;
    [SerializeField]
    private float _maxPitchSpeed = 100;
    [SerializeField]
    private float _maxRollSpeed = 70;

    //Controls
    private float _horizontalValue;
    private float _verticalValue;
    private float _rotateValue;
    private bool _impulse;
    private float _stopImpulse;
    private float _nextImpulse;
    private float _yaw;

    //Components
    private Animator _animator;
    private Rigidbody _rb;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _rb.velocity = Vector3.zero;
    }
    private void Update()
    {
        InputUpdate();
        Turn();
        Animation();
    }

    private void FixedUpdate()
    {
        MoveUpdate();        
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {//Active Compass
            _compass.ChangeMemoryStates();
        }
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

        if (_yaw == 0)
            _rb.velocity = transform.forward * newBoost * _speed * Time.fixedDeltaTime;
    }

    private void Turn()
    {
        _yaw = _turnYawSpeed * Time.deltaTime * _horizontalValue;
        print("X: " + transform.rotation.eulerAngles);
        if(transform.rotation.eulerAngles.x <= 45 && transform.rotation.eulerAngles.x > -45)
        {
            float pitch = _turnPitchSpeed * Time.deltaTime * -_verticalValue;
            transform.Rotate(pitch, 0, 0);
        }else if (transform.rotation.eulerAngles.x > 45)
        {
            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(40, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime);
        }
        else if(transform.rotation.eulerAngles.x < -45)
        {
            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(-40, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime);
        }
        float roll = _turnRollSpeed * Time.deltaTime * _rotateValue;

        transform.RotateAround(yaw < 0 ? rightPoint.position : leftPoint.position, Vector3.up, yaw);
        //transform.Rotate(pitch, 0, roll);
    }

    private void Animation()
    {
        _animator.SetBool("Space", Input.GetKey("space"));
        _animator.SetBool("Left", _horizontalValue < 0 ? true : false);
        _animator.SetBool("Right", _horizontalValue > 0 ? true : false);
        _animator.SetBool("Up", _verticalValue > 0 ? true : false);
        _animator.SetBool("Down", _verticalValue < 0 ? true : false);
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

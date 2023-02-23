using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //REFERENCES
    [Header("Referenes")]
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputMovement;
    private Rigidbody _rb;
    private Animator _animator;

    // CONFIGURATION
    [Header("Configuration")]
    [SerializeField] private float _turnSpeed = 60f;
    [SerializeField] private float _moveSpeed = 45f;
    [SerializeField] private float _dashBoost = 2f;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;

    // INPUTS VALUES
    private float _horizontalValue;
    private float _verticalValue;
    private float _rotateValue;
    private float _dashBtn;

    // PLAYER STATES
    private float _finishDash;
    private float _nextDash = 0;
    public enum WHALE_STATE
    {
        move = 0,
        paht = 1,
        dash = 2
    }
    [SerializeField] private WHALE_STATE _whaleState = WHALE_STATE.move;

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
        CheckCooldowns();
        Inputs();
        Turn();
        Thrust();
        Animation();
    }
    /// <summary>
    /// Manage the cooldowns
    /// </summary>
    private void CheckCooldowns()
    {
        if (_whaleState == WHALE_STATE.dash && Time.realtimeSinceStartup >= _finishDash)
        {
            _whaleState = WHALE_STATE.move;
            _nextDash = Time.realtimeSinceStartup + _dashCooldown;
        }
    }
    /// <summary>
    /// Manage the inputs in Gameplay ActionMap
    /// </summary>
    private void Inputs()
    {
        switch (_whaleState)
        {
            case WHALE_STATE.move:
                InputsMove();
                break;
            case WHALE_STATE.paht:
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// Manage the inputs when the whale is move
    /// </summary>
    private void InputsMove()
    {
        //INPUTS VALUES
        _inputMovement = _playerInputActions.Gameplay.Movement.ReadValue<Vector2>();
        _horizontalValue = _inputMovement.x;
        _verticalValue = _inputMovement.y;
        _rotateValue = _playerInputActions.Gameplay.Rotate.ReadValue<float>();
        _dashBtn = _playerInputActions.Gameplay.Dash.ReadValue<float>();

        //INPUT ACTIONS
        if (_dashBtn != 0 && Time.realtimeSinceStartup >= _nextDash)
        {
            _whaleState = WHALE_STATE.dash;
            _finishDash = Time.realtimeSinceStartup + _dashDuration;
        }
    }
    /// <summary>
    /// Manage the rotations
    /// </summary>
    private void Turn()
    {
        float yaw = _turnSpeed * Time.fixedDeltaTime * _horizontalValue;
        float pitch = _turnSpeed * Time.fixedDeltaTime * _verticalValue;
        float roll = _turnSpeed * Time.fixedDeltaTime * _rotateValue;
        transform.Rotate(-1 * pitch, yaw, roll);
    }
    /// <summary>
    /// Manage the movement
    /// </summary>
    private void Thrust()
    {
        float boost;
        if (_whaleState == WHALE_STATE.dash)
        {
            boost = _moveSpeed * _dashBoost;
        }
        else
        {
            boost = _moveSpeed;
        }
        transform.position += transform.forward * boost * Time.fixedDeltaTime;
    }
    /// <summary>
    /// Manage Animations using:
    /// <see cref="_horizontalValue"/>
    /// <see cref="_verticalValue"/>
    /// </summary>
    private void Animation()
    {
        _animator.SetBool("Space", _dashBtn != 0 ? true : false);
        _animator.SetBool("Left", _horizontalValue < 0 ? true : false);
        _animator.SetBool("Right", _horizontalValue > 0 ? true : false);
        _animator.SetBool("Up", _verticalValue > 0 ? true : false);
        _animator.SetBool("Down", _verticalValue < 0 ? true : false);
    }
    /// <summary>
    /// Return's <see cref="_playerInputActions"/>
    /// </summary>
    /// <returns></returns>
    public PlayerInputActions GetPlayerInputActions()
    {
        return _playerInputActions;
    }
    /// <summary>
    /// Set's <see cref="_playerInputActions"/> to Gameplay Action Map
    /// </summary>
    public void SetInputActionGameplay()
    {
        _playerInputActions.Gameplay.Enable();
    }
    /// <summary>
    /// Set's <see cref="_playerInputActions"/> to Paths Action Map
    /// </summary>
    public void SetInputActionPaths()
    {
        _playerInputActions.Paths.Enable();
    }
}

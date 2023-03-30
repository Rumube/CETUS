using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    //REFERENCES
    [Header("References")]
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputMovement;
    private Rigidbody _rb;
    private Animator _animator;
    private WhalePahtController _pathController;
    [SerializeField] private List<StudioEventEmitter> _whaleSounds;
    [SerializeField] private StudioEventEmitter _whaleSprint;
    [SerializeField] private GameObject _dashCamera;
    [SerializeField] private GameObject _menu;

    // CONFIGURATION
    [Header("Movement Configuration")]
    [Range(0.1f, 3f)]
    [SerializeField] private float _movementDelay = 1f;
    [SerializeField] private float _turnSpeed = 60f;
    [SerializeField] private float _moveSpeed = 45f;

    private float _lastYaw = 0;
    private float _lastPitch = 0;

    [Header("Dash Configuration")]
    [SerializeField] private float _dashBoost = 2f;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;
    [Header("Sounds")]
    [SerializeField] private float _whaleSoundsDelay = 5.0f;
    [Range(10, 100)]
    [SerializeField] private float _whaleSoundFrecuency = 5.0f;
    private bool _canPlaySound = true;
    private int _lastSound = 0;
    private float speedYaw = 0;
    private float speedPitch = 0;

    // INPUTS VALUES
    private float _horizontalValue;
    private float _verticalValue;
    private float _rotateValue;
    private float _dashBtn;

    // PLAYER STATES
    private float _finishDash;
    private float _nextDash = 0;
    private int _lastState = 0;
    public enum WHALE_STATE
    {
        move = 0,
        paht = 1,
        dash = 2,
        wormhole=3,
        pause = 4
    }
    [SerializeField] private WHALE_STATE _whaleState = WHALE_STATE.move;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerInputActions = new PlayerInputActions();
        _pathController = GetComponent<WhalePahtController>();

        SwitchActionMap(WHALE_STATE.move);
    }

    private void Start()
    {
        _rb.useGravity = false;

        // Releases the cursor
        Cursor.lockState = CursorLockMode.None;
        // Locks the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Confines the cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

    }
    private void Update()
    {
        ChooseWhaleSound();
    }

    private void FixedUpdate()
    {
        CheckCooldowns();
        Inputs();
        if (_whaleState != WHALE_STATE.pause)
        {
            Turn();
            Thrust();
        }
        Animation();
    }
    /// <summary>
    /// Manage the cooldowns
    /// </summary>
    private void CheckCooldowns()
    {
        if (_whaleState == WHALE_STATE.dash && Time.realtimeSinceStartup >= _finishDash)
        {
            SwitchActionMap(WHALE_STATE.move);
            _nextDash = Time.realtimeSinceStartup + _dashCooldown;
            _dashCamera.SetActive(false);
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
                InputsPath();
                break;
            case WHALE_STATE.wormhole:
                SetStartRotation();
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
        if (_whaleState != WHALE_STATE.dash && _dashBtn != 0 && Time.realtimeSinceStartup >= _nextDash)
        {
            if (_whaleSprint.IsPlaying()) _whaleSprint.Stop();
            _whaleSprint.Play();
            SwitchActionMap(WHALE_STATE.dash);
            _finishDash = Time.realtimeSinceStartup + _dashDuration;
            _dashCamera.SetActive(true);
        }
    }
    /// <summary>
    /// Manage the inputs when the whale is in path
    /// </summary>
    private void InputsPath()
    {
        _pathController.UpdatePath();
    }
    /// <summary>
    /// Sets te rotations to zero
    /// </summary>
    private void SetStartRotation()
    {
        transform.rotation = transform.rotation = Quaternion.Euler(0, 0, 0); 
    }
    /// <summary>
    /// Manage the rotations
    /// </summary>
    private void Turn()
    {
        float yaw = _turnSpeed * Time.fixedDeltaTime * _horizontalValue;
        float pitch = _turnSpeed * Time.fixedDeltaTime * _verticalValue;
        float roll = _turnSpeed * Time.fixedDeltaTime * _rotateValue;

        float rampUp = 0.2f;

        if (yaw < 0.2f && yaw > -0.2f)
        {
            speedYaw = 0;
        }
        else
        {
            speedYaw += yaw * rampUp * Time.deltaTime;
        }


        if (pitch < 0.2f && pitch > -0.2f)
        {
            speedPitch = 0;
        }
        else
        {
            speedPitch += pitch * rampUp * Time.deltaTime;
        }

        speedYaw = Mathf.Clamp(speedYaw, -1, 1);
        speedPitch = Mathf.Clamp(speedPitch, -1, 1);

        transform.Rotate(-1 * speedPitch, speedYaw, roll);
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
        _rb.velocity = transform.forward * boost * Time.fixedDeltaTime;
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

    //GETTER

    /// <summary>
    /// Returns the state of the whale
    /// </summary>
    /// <returns><see cref="_whaleState"/></returns>
    public WHALE_STATE GetWhaleState()
    {
        return _whaleState;
    }

    //SETTER

    /// <summary>
    /// Sets the value of <see cref="_whaleState"/>.
    /// The whale state
    /// </summary>
    /// <param name="whaleState">new whale state</param>
    public void SetWhaleState(WHALE_STATE whaleState)
    {
        _whaleState = whaleState;
    }
    /// <summary>
    /// Change the <see cref="_whaleState"/> and the InputAction
    /// </summary>
    /// <param name="whaleState">New State</param>
    public void SwitchActionMap(WHALE_STATE whaleState)
    {
        SetWhaleState(whaleState);
        switch (whaleState)
        {
            case WHALE_STATE.move:
            case WHALE_STATE.dash:
                _playerInputActions.Gameplay.Enable();
                break;
            case WHALE_STATE.paht:
                _playerInputActions.Paths.Enable();
                break;
            default:
                break;
        }
    }

    #region GETTERS
    public float GetVerticalAxis()
    {
        return _verticalValue;
    }
    #endregion

    #region SETTERS

    public void SetPause()
    {
        if (_whaleState != WHALE_STATE.pause)
        {
            _lastState = (int)_whaleState;
            _whaleState = WHALE_STATE.pause;
            _rb.velocity = Vector3.zero;
            _menu.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
            _whaleState = (WHALE_STATE)_lastState;
            _menu.SetActive(false);
            Cursor.visible = false;
        }
    }
    #endregion


    #region Sounds
    /// <summary>
    /// Choose a sound for the whale with a delay
    /// </summary>
    private void ChooseWhaleSound()
    {
        if (_canPlaySound && UnityEngine.Random.Range(0, 100) >= (100 - _whaleSoundFrecuency))
        {
            for (int i = 0; i < _whaleSounds.Count; ++i)
            {
                if (_whaleSounds[i].IsPlaying())
                {
                    return;
                }
            }
            StudioEventEmitter soundChoosen = _whaleSounds[_lastSound];

            if (_lastSound == 0) _lastSound = 1;
            else _lastSound = 0;

            soundChoosen.Play();
            StartCoroutine(DelaySounds());
        }
    }
    private IEnumerator DelaySounds()
    {
        _canPlaySound = false;
        yield return new WaitForSeconds(_whaleSoundsDelay);
        _canPlaySound = true;
    }

    #endregion
}

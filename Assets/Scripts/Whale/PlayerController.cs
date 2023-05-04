using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //REFERENCES
    [Header("References")]
    private PlayerInputActions _playerInputActions;
    private Vector2 _inputMovement;
    private Rigidbody _rb;
    private Button _playBtn;
    private WhalePahtController _pathController;

    [SerializeField] private Animator _animator;
    [SerializeField] private List<StudioEventEmitter> _whaleSounds;
    [SerializeField] private StudioEventEmitter _whaleSprint;
    [SerializeField] private GameObject _dashCamera;
    [SerializeField] private GameObject _initialCamera;
    [SerializeField] private GameObject _menu;

    // CONFIGURATION
    [Header("Movement Configuration")]
    [Range(0.1f, 300f)]
    [SerializeField] private float _turnSpeed = 100f;
    [SerializeField] private float _moveSpeed = 45f;
    [SerializeField] private float _moveDelay = 0.2f;

    [Header("Dash Configuration")]
    [SerializeField] private float _dashBoost = 2f;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;
    [Header("Sounds")]
    [SerializeField] private float _whaleSoundsDelay = 5.0f;
    [Range(10, 100)]
    [SerializeField] private float _whaleSoundFrecuency = 5.0f;
    private int _invertValue = -1;
    private bool _canPlaySound = true;
    private int _lastSound = 0;
    private float speedYaw = 0;
    private float speedPitch = 0;

    // INPUTS VALUES
    private float _horizontalValue;
    private float _verticalValue;
    private float _rotateValue;
    private float _dashBtn;
    private float _pitch;

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
        pause = 4,
        initGame = 5
    }
    [SerializeField] private WHALE_STATE _whaleState = WHALE_STATE.initGame;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerInputActions();
        _pathController = GetComponent<WhalePahtController>();
        if (GameObject.FindGameObjectWithTag("PlayButton"))
        {
            _playBtn = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
        }
        StartCoroutine(StartWait());
    }

    private void Start()
    {
        _rb.useGravity = false;
        StartCoroutine(DesactivateInitialCamera());
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

    private IEnumerator DesactivateInitialCamera()
    {
        yield return new WaitForSeconds(3f);
        _initialCamera.SetActive(false);
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
            _animator.SetBool("Space", true);
            StartCoroutine(StopDash());
            SwitchActionMap(WHALE_STATE.dash);
            _finishDash = Time.realtimeSinceStartup + _dashDuration;
            _dashCamera.SetActive(true);
        }
    }

    private IEnumerator StopDash()
    {
        yield return 0;
        _animator.SetBool("Space", false);
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

        

        if (yaw < 0.1f && yaw > -0.1f)
        {
            speedYaw = 0;
        }
        else
        {
            speedYaw += yaw * _moveDelay * Time.deltaTime;
        }


        if (pitch < 0.1f && pitch > -0.1f)
        {
            speedPitch = 0;
        }
        else
        {
            speedPitch += pitch * _moveDelay * Time.deltaTime;
        }

        speedYaw = Mathf.Clamp(speedYaw, -1, 1);
        speedPitch = Mathf.Clamp(speedPitch, -1, 1);

        transform.Rotate(_invertValue * speedPitch, speedYaw, roll);
        _pitch = speedYaw;
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
    /// </summary>
    private void Animation()
    {
        _animator.SetFloat("Pitch", _pitch);

        //if (_animator.GetBool("Left") || _animator.GetBool("Right"))
        //{
        //    if (_animator.GetBool("Left"))
        //    {
        //        _animator.SetBool("StopTurnLeft", _horizontalValue > 0.3f ? true : false);
        //        _animator.SetBool("Left", false);
        //    }
        //    else if (_animator.GetBool("Right"))
        //    {
        //        _animator.SetBool("StopTurnRight", _horizontalValue < -0.3f ? true : false);
        //        _animator.SetBool("Left", false);
        //    }
        //}
        //else
        //{
        //    _animator.SetBool("Left", _horizontalValue < -0.3f ? true : false);
        //    _animator.SetBool("Right", _horizontalValue > 0.3f ? true : false);
        //}
        //_animator.SetBool("Up", _verticalValue > 0 ? true : false);
        //_animator.SetBool("Down", _verticalValue < 0 ? true : false);
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

    private IEnumerator StartWait()
    {
        SwitchActionMap(WHALE_STATE.initGame);
        yield return new WaitForSeconds(6f);
        SwitchActionMap(WHALE_STATE.move);
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
            _playBtn.Select();
            // Confines the cursor
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            _whaleState = (WHALE_STATE)_lastState;
            _menu.SetActive(false);
            // Confines the cursor
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    public void SetInvertValue(int value)
    {
        _invertValue = value;
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

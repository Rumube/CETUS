using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.InputSystem;


public class WhalePahtController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _pathCamera;
    private PathCreator _pathcreator;
    private PathController _pathController;

    [Header("PlayerController")]
    private PlayerController _playerController;
    private PlayerInputActions _inputActions;
    [Tooltip("Ease with which cetus gets out of the paths, the lower it is, the easier it gets out of them.")]
    [SerializeField][Range(0f, 1f)] private float _exitSensitivity;
    [Tooltip("Ease with which cetus changes direction, the lower the easier it changes direction.")]
    [SerializeField][Range(0f, 1f)] private float _changeDirectionSensitivity;
    [Header("Status")]
    private bool _isPath = false;
    [Tooltip("Initial entry direction. True = Strart to Final / False = Final to Start")]
    private bool _enterDirection = false;
    private bool _direction = true;

    [Header("Travel values")]
    [SerializeField] private bool _initPath;
    public float _speed = 0;
    float _distanceTravelled = 0;
    public float MINSPEED = 10;


    //Input Values
    public float _exitTime = 0;
    public float _timeToNextTravel = 0;

    private float _verAxis = 0;
    private float _horAxis = 0;
    private float _nextExit = 0;
    private float _nextTravel = 0;
    private bool _isExit = false;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _inputActions = _playerController.GetPlayerInputActions();
    }

    /// <summary>
    /// Manage the inputs when the whale is in path
    /// </summary>
    public void UpdatePath()
    {
        if (!_isPath)
        {
            UpdateInputs();
            ExitConfiguration();
        }
        UpdateInPath();
    }
    /// <summary>
    /// Manage the inputs
    /// </summary>
    private void UpdateInputs()
    {
        if (_inputActions == null)
        {
            _inputActions = _playerController.GetPlayerInputActions();
        }


        if (_inputActions.Paths.Test.IsPressed())
        {
            Debug.Log("TEST");
        }

        _horAxis = _inputActions.Paths.Direction.ReadValue<Vector2>().x;
        _verAxis = _inputActions.Paths.Direction.ReadValue<Vector2>().y;
    }

    /// <summary>
    /// Use the <see cref="_horAxis"/> and <see cref="_exitTime"/>
    /// values to check if the player want to leave the path.
    /// </summary>
    private void ExitConfiguration()
    {
        if (_horAxis >= _exitSensitivity || _horAxis <= -_exitSensitivity)
        {
            if (!_isExit)
            {
                _isExit = true;
                _nextExit = Time.realtimeSinceStartup + _exitTime;
            }
            else if (Time.realtimeSinceStartup >= _nextExit)
            {
                GetOutPath();
            }
        }
        else
        {
            _isExit = false;
        }
    }

    /// <summary>
    /// Manage the movement in path
    /// </summary>
    private void UpdateInPath()
    {
        if (_isPath)
        {
            float playerDirection = _verAxis;
            SetDirection(playerDirection);
            playerDirection = CorrectVelocity(playerDirection);
            if (_direction)
            {
                _distanceTravelled += (playerDirection * _speed * Time.deltaTime) + MINSPEED;
            }
            else
            {
                _distanceTravelled += (playerDirection * _speed * Time.deltaTime) - MINSPEED;
            }
            transform.position = _pathcreator.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
            transform.rotation = _pathcreator.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);

            if (!_direction)
            {
                transform.forward *= -1;
            }
        }
    }

    /// <summary>
    /// Set the player direcction, if the vertical axis is
    /// less than -0.5f <see cref="_direction"/> is FALSE and if the vertical axis is
    /// gratter than 0.5f <see cref="_direction"/> is TRUE
    /// </summary>
    /// <param name="playerVel"></param>
    private void SetDirection(float playerVel)
    {
        if (playerVel <= -_changeDirectionSensitivity)
        {
            _direction = false;
        }
        else if (playerVel >= _changeDirectionSensitivity)
        {
            _direction = true;
        }
    }
    /// <summary>
    /// Force the variable <paramref name="playerVel"/> to set
    /// different than 0 to move always the player.
    /// </summary>
    /// <param name="playerVel"></param>
    /// <returns></returns>
    private float CorrectVelocity(float playerVel)
    {
        if (_direction && playerVel == 0)
        {
            playerVel = 0.1f;
        }
        else if (!_direction && playerVel == 0)
        {
            playerVel = -0.1f;
        }
        return playerVel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PathGuide" && !_isPath && Time.realtimeSinceStartup >= _nextTravel)
        {
            EnterInPath(other);
        }
        else if (other.tag == "PathFinish" && _isPath)
        {
            GetOutPath();
        }
    }
    /// <summary>
    /// ActionMap and whale status are changed to run on the road.
    /// </summary>
    /// <param name="other">The collider with which the collision occurred</param>
    private void EnterInPath(Collider other)
    {
        _playerController.SwitchActionMap(PlayerController.WHALE_STATE.paht);
        _isPath = true;
        _playerController.SetInputActionPaths();
        _pathcreator = other.gameObject.GetComponentInParent<PathCreator>();
        _pathController = other.gameObject.GetComponentInParent<PathController>();
        _distanceTravelled = _pathcreator.path.GetClosestDistanceAlongPath(transform.position);
        if(!_initPath)
        {
            _pathCamera.SetActive(true);
        }
        SetInitDirection();
    }
    /// <summary>
    /// Change ActionMap and whale status to get out of the way.
    /// </summary>
    private void GetOutPath()
    {
        _isExit = false;
        _isPath = false;
        _playerController.SwitchActionMap(PlayerController.WHALE_STATE.move);
        _nextTravel = Time.realtimeSinceStartup + _timeToNextTravel;
        _pathCamera.SetActive(false);
    }

    /// <summary>
    /// Compare the distances between cetus and the two entrances and choose the closest one.
    /// If the result is true = end direction.
    /// If the result is false = direction to start.
    /// </summary>
    private void SetInitDirection()
    {
        float distanceBetweenCetusInit = Vector3.Distance(transform.position, _pathController.GetFinishPaths()[0].transform.position);
        float distanceBetweenCetusFinish = Vector3.Distance(transform.position, _pathController.GetFinishPaths()[1].transform.position);

        if (distanceBetweenCetusInit < distanceBetweenCetusFinish)
        {
            _enterDirection = true;
            _direction = true;
        }
        else
        {
            _direction = false;
            _enterDirection = false;
        }
    }
}

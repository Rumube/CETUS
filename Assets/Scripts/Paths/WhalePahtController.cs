using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class WhalePahtController : MonoBehaviour
{
    [Header("Status")]
    private bool _isPath = false;
    [SerializeField]
    private bool _direction = true;

    [Header("Travel values")]
    public float _speed = 0;
    float _distanceTravelled = 0;
    private const float MINSPEED = 10;

    private PathCreator _pathcreator;

    //Input Values
    public float _exitTime = 0;
    public float _timeToNextTravel = 0;

    private float _verAxis = 0;
    private float _horAxis = 0;
    private float _nextExit = 0;
    private float _nextTravel = 0;
    private bool _isExit = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
        ExitConfiguration();
        UpdateInPath();
    }
    /// <summary>
    /// Manage the inputs
    /// </summary>
    private void UpdateInputs()
    {
        _verAxis = Input.GetAxis("Vertical");
        _horAxis = Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// Use the <see cref="_horAxis"/> and <see cref="_exitTime"/>
    /// values to check if the player want to leave the path.
    /// </summary>
    private void ExitConfiguration()
    {
        if (_horAxis >= 0.9f || _horAxis <= -0.9f)
        {
            if (!_isExit)
            {
                _isExit = true;
                _nextExit = Time.realtimeSinceStartup + _exitTime;
            }else if(Time.realtimeSinceStartup >= _nextExit)
            {
                _isExit = false;
                _isPath = false;
                _nextTravel = Time.realtimeSinceStartup + _timeToNextTravel;
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
        if (playerVel <= -0.5f)
        {
            _direction = false;
        }
        else if (playerVel >= 0.5f)
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
        if(other.tag == "PathGuide" && !_isPath)
        {
            if(Time.realtimeSinceStartup >= _nextTravel)
            {
                _isPath = true;
                _pathcreator = other.gameObject.GetComponentInParent<PathCreator>();
                _distanceTravelled = _pathcreator.path.GetClosestDistanceAlongPath(transform.position);
            }
        }
    }
}

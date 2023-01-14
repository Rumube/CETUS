using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class WhalePahtController : MonoBehaviour
{
    [Header("Status")]
    private bool _isPath = false;
    private bool _direction = true;

    [Header("Travel values")]
    public PathCreator _pathcreator;
    public float _speed = 1;
    float _distanceTravelled;
    public const float MINSPEED = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.C)) {
            _isPath = !_isPath;
            _distanceTravelled = _pathcreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        if (_isPath)
        {
            float playerVel = Input.GetAxis("Vertical");
            _distanceTravelled += (playerVel * _speed) + (MINSPEED * Time.deltaTime);
            transform.position = _pathcreator.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
            transform.rotation = _pathcreator.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
        }
    }
}

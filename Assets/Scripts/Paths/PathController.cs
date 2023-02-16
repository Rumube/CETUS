using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    //Values
    private float _distanceTravelled = 0;
    [Header("References")]
    //References
    public GameObject _guide;
    private GameObject _player;
    private PathCreator _pathcreator;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _pathcreator = GetComponent<PathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        _distanceTravelled = _pathcreator.path.GetClosestDistanceAlongPath(_player.transform.position);
        _guide.transform.position = _pathcreator.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
        _guide.transform.rotation = _pathcreator.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
    }
}

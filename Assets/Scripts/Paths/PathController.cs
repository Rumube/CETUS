using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    //Values
    [SerializeField] private bool _nexoPath;
    private float _distanceTravelled = 0;
    [Header("References")]
    //References
    [SerializeField]private GameObject _guide;
    [SerializeField]private GameObject[] _finishPaths = new GameObject[2];
    private GameObject _player;
    private PathCreator _pathcreator;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _pathcreator = GetComponent<PathCreator>();
        _finishPaths[0].transform.position = _pathcreator.path.GetPoint(0);
        _finishPaths[1].transform.position = _pathcreator.path.GetPoint(_pathcreator.path.NumPoints - 1);
    }

    // Update is called once per frame
    void Update()
    {
        _distanceTravelled = _pathcreator.path.GetClosestDistanceAlongPath(_player.transform.position);
        _guide.transform.position = _pathcreator.path.GetPointAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
        _guide.transform.rotation = _pathcreator.path.GetRotationAtDistance(_distanceTravelled, EndOfPathInstruction.Stop);
    }

    #region GETTER
    public GameObject[] GetFinishPaths()
    {
        return _finishPaths;
    }
    public bool GetNexoPath()
    {
        return _nexoPath;
    }
    #endregion
}

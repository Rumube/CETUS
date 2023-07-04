using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexoManager : MonoBehaviour
{

    private GameObject _player;
    [Header("Configuration")]
    [SerializeField, Range(0.0f, 1.0f)] float _requiredFragmentsZone1 = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] float _requiredFragmentsZone3 = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] float _requiredFragmentsZone6 = 0.8f;

    [Header("Current Fragments")]
    [SerializeField] private int _fragmentsNumZone1 = 0;
    [SerializeField] private int _fragmentsNumZone3 = 0;
    [SerializeField] private int _fragmentsNumZone6 = 0;

    [Header("Neded Fragments")]
    [SerializeField] private int _fragmentsToFinishZone1 = 0;
    [SerializeField] private int _fragmentsToHalfZone1 = 0;
    [SerializeField] private int _fragmentsToFinishZone3 = 0;
    [SerializeField] private int _fragmentsToHalfZone3 = 0;
    [SerializeField] private int _fragmentsToFinishZone6 = 0;
    [SerializeField] private int _fragmentsToHalfZone6 = 0;

    [Header("Animations References")]
    private AnimationNexo _animationNexo;

    [Header("BlackHoles")]
    [SerializeField] private GameObject[] _blackHoles;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _fragmentsNumZone1 = GameObject.FindGameObjectsWithTag("FragmentsZone1").Length;
        _fragmentsNumZone3 = GameObject.FindGameObjectsWithTag("FragmentsZone3").Length;
        _fragmentsNumZone6 = GameObject.FindGameObjectsWithTag("FragmentsZone6").Length;

        _fragmentsToFinishZone1 = (int)(_fragmentsNumZone1 - _fragmentsNumZone1 * Mathf.Clamp01(_requiredFragmentsZone1));
        _fragmentsToHalfZone1 = (int)(_fragmentsToFinishZone1 + (_fragmentsNumZone1 - _fragmentsToFinishZone1) / 2);

        _fragmentsToFinishZone3 = (int)(_fragmentsNumZone3 - _fragmentsNumZone3 * Mathf.Clamp01(_requiredFragmentsZone3));
        _fragmentsToHalfZone3 = (int)(_fragmentsToFinishZone3 + (_fragmentsNumZone3 - _fragmentsToFinishZone3) / 2);

        _fragmentsToFinishZone6 = (int)(_fragmentsNumZone6 - _fragmentsNumZone6 * Mathf.Clamp01(_requiredFragmentsZone6));
        _fragmentsToHalfZone6 = (int)(_fragmentsToFinishZone6 + (_fragmentsNumZone6 - _fragmentsToFinishZone6) / 2);

        _animationNexo = GetComponent<AnimationNexo>();
    }

    public void FragmentProvided(CollectedMemory.Zone zone)
    {
        switch (zone)
        {
            case CollectedMemory.Zone.zone1:
                _fragmentsNumZone1--;
                CheckZone1();
                break;
            case CollectedMemory.Zone.zone3:
                _fragmentsNumZone3--;
                CheckZone3();
                break;
            case CollectedMemory.Zone.zone6:
                _fragmentsNumZone6--;
                CheckZone6();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Checks if it is necessary to activate animations and to allow level changes Zone1
    /// </summary>
    private void CheckZone1()
    {
        if (_fragmentsNumZone1 == _fragmentsToHalfZone1)
        {
            StartCoroutine(_animationNexo.StartMemory1());
        }
        else if (_fragmentsNumZone1 == _fragmentsToFinishZone1)
        {
            StartCoroutine(_animationNexo.StartMemory2());
            _blackHoles[0].SetActive(true);
            _player.GetComponent<Limitation>().incressMaxLvl();
        }
    }

    private void CheckZone3()
    {
        if (_fragmentsNumZone3 == _fragmentsToHalfZone3)
        {
            StartCoroutine(_animationNexo.StartMemory3());
        }
        else if (_fragmentsNumZone3 == _fragmentsToFinishZone3)
        {
            _blackHoles[1].SetActive(true);
            _player.GetComponent<Limitation>().incressMaxLvl();
            StartCoroutine(_animationNexo.StartMemory4());
        }
    }

    private void CheckZone6()
    {
        if (_fragmentsNumZone6 == _fragmentsToHalfZone6)
        {
            StartCoroutine(_animationNexo.StartMemory5());
        }
        else if (_fragmentsNumZone6 == _fragmentsToFinishZone6)
        {
            _player.GetComponent<Limitation>().incressMaxLvl();
            StartCoroutine(_animationNexo.StartMemory6());
        }
    }
}

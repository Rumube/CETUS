using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexoManager : MonoBehaviour
{
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
    [SerializeField] private int _fragmentsToFinishZone3 = 0;
    [SerializeField] private int _fragmentsToHalfZone3 = 0;
    [SerializeField] private int _fragmentsToFinishZone6 = 0;

    [Header("Animations References")]
    private AnimationNexo _animationNexo;

    private void Awake()
    {
        _fragmentsNumZone1 = GameObject.FindGameObjectsWithTag("FragmentsZone1").Length;
        //_fragmentsNumZone3 = GameObject.FindGameObjectsWithTag("FragmentsZone3").Length;
        _fragmentsNumZone6 = GameObject.FindGameObjectsWithTag("FragmentsZone6").Length;

        _fragmentsToFinishZone1 = (int)(_fragmentsNumZone1 - _fragmentsNumZone1 * Mathf.Clamp01(_requiredFragmentsZone1));
        _fragmentsToFinishZone3 = (int)(_fragmentsNumZone3 - _fragmentsNumZone3 * Mathf.Clamp01(_requiredFragmentsZone3));
        _fragmentsToHalfZone3 = (int)(_fragmentsToFinishZone3 + (_fragmentsNumZone3 - _fragmentsToFinishZone3) / 2);
        _fragmentsToFinishZone6 = (int)(_fragmentsNumZone6 - _fragmentsNumZone6 * Mathf.Clamp01(_requiredFragmentsZone6));

        _animationNexo = GetComponent<AnimationNexo>();
    }

    // Update is called once per frame
    void Update()
    {

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

    private void CheckZone1()
    {
        if (_fragmentsNumZone1 == _fragmentsToFinishZone1)
        {
            StartCoroutine(_animationNexo.StartMemory1());
        }
    }

    private void CheckZone3()
    {
        if (_fragmentsNumZone3 == _fragmentsToHalfZone3)
        {
            StartCoroutine(_animationNexo.StartMemory2());
        }else if(_fragmentsNumZone3 == _fragmentsToFinishZone3)
        {
            StartCoroutine(_animationNexo.StartMemory3());
        }
    }

    private void CheckZone6()
    {
        if (_fragmentsNumZone6 == _fragmentsToFinishZone6)
        {
            StartCoroutine(_animationNexo.StartMemory4());
        }
    }
}

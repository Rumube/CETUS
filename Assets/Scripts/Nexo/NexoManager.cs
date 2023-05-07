using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexoManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField, Range(0.0f, 1.0f)] float _requiredFragmentsZone1 = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] float _requiredFragmentsZone3 = 0.8f;
    [SerializeField, Range(0.0f, 1.0f)] float _requiredFragmentsZone6 = 0.8f;

    [SerializeField] private int _fragmentsNumZone1 = 0;
    [SerializeField] private int _fragmentsNumZone3 = 0;
    [SerializeField] private int _fragmentsNumZone6 = 0;

    private void Awake()
    {
        _fragmentsNumZone1 = GameObject.FindGameObjectsWithTag("FragmentsZone1").Length;
        _fragmentsNumZone3 = GameObject.FindGameObjectsWithTag("FragmentsZone3").Length;
        _fragmentsNumZone6 = GameObject.FindGameObjectsWithTag("FragmentsZone6").Length;
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
                break;
            case CollectedMemory.Zone.zone6:
                _fragmentsNumZone6--;
                break;
            default:
                break;
        }   
    }

    private void CheckZone1()
    {
        if( _fragmentsNumZone1 == _fragmentsNumZone1 - _fragmentsNumZone1 * Mathf.Clamp01(_requiredFragmentsZone1))
        {

        }
    }
}

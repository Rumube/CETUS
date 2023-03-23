using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("Rotation configuration")]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _detectionDist;
    [SerializeField] private float _rotationSpeed = 10;

    [Header("References")]
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        CollisionDetection();
    }

    private void CollisionDetection()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _detectionDist, _layerMask))
        {
            if (_playerController.GetVerticalAxis() > 0)
            {
                transform.Rotate(-1 * _rotationSpeed * Time.deltaTime * 1, 0, 0);
            }
            else
            {
                transform.Rotate(_rotationSpeed * Time.deltaTime * 1, 0, 0);
            }
        }
    }
}

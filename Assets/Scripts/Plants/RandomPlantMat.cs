using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlantMat : MonoBehaviour
{
    [SerializeField, Range(0,100)] private float _chanceGlow;
    [SerializeField] private Material[] _posibleMats;
    [SerializeField] private MeshRenderer _renderer;

    private void Awake()
    {
        SetNewMat();
    }

    private void SetNewMat()
    {
        int randomValue = Random.Range(0, 100);
        if(randomValue < _chanceGlow)
        {
            _renderer.material = _posibleMats[0];
        }
        else
        {
            _renderer.material = _posibleMats[1];
        }
    }

}

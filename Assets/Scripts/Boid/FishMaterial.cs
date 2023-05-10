using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaterial : MonoBehaviour
{
    [SerializeField] Texture2D[] _posibleTextures;
    [SerializeField, ColorUsageAttribute(true, true)] Color[] _posibleColors;
    private Material _material;
    [SerializeField] bool _isTurtle;

    private void Awake()
    {
        if(!_isTurtle)
        {
            _material = GetComponent<Renderer>().material;
            SetFuerza(0.4f);
            int random = Random.Range(0, _posibleTextures.Length);
            _material.SetTexture("_Textura", _posibleTextures[random]);
            _material.SetColor("_ColorEmision", _posibleColors[random]);
        }
        else
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.materials[0].mainTexture = _posibleTextures[Random.Range(0,_posibleTextures.Length)];
        }

    }

    public void SetFuerza(float fuerza)
    {
        _material.SetFloat("_Fuerza", fuerza);
    }
}

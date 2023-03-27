using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaterial : MonoBehaviour
{
    [SerializeField] Texture2D[] _posibleTextures;
    [SerializeField][ColorUsage(true, true, 0f, 8f, 0.125f, 3f)][System.Obsolete] Color[] _posibleColors;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        SetFuerza(0.4f);
        int random = Random.Range(0,_posibleTextures.Length);
        _material.SetTexture("_Textura", _posibleTextures[random]);
        _material.SetColor("_Color", _posibleColors[random]);
    }

    public void SetFuerza(float fuerza)
    {
        _material.SetFloat("_Fuerza", fuerza);
    }
}

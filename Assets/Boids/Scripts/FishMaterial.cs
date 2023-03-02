using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMaterial : MonoBehaviour
{
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        SetFuerza(0.4f);
    }

    public void SetFuerza(float fuerza)
    {
        _material.SetFloat("_Fuerza", fuerza);
    }
}

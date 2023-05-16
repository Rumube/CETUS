using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlackHole : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _isNextLevel = false;
    private GameObject _cetus;
    private void Awake()
    {
        _cetus = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("Entra");
            _cetus.GetComponent<Limitation>().StartTeleport(_isNextLevel);
        }
    }
}

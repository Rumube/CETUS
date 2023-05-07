using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNexo : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _showingMemory = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _animator.SetBool("Beat", false);
    }
    public void StartAnimOpen()
    {
        _showingMemory = true;
        _animator.Play("Nexo_Animation01_Zone0");
    }
    public void StartAnimBeat()
    {
        if (!_showingMemory)
        {
            _animator.SetBool("Beat", true);
        }
    }


}

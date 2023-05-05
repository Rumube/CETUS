using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNexo : MonoBehaviour
{
    [SerializeField] private Animator _animator;

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
        _animator.Play("Nexo_Animation01_Zone0");
    }
    public void StartAnimBeat()
    {
        _animator.SetBool("Beat", true);
    }
}

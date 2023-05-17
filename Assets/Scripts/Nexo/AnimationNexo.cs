using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNexo : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator[] _memorysAnims;

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

    public IEnumerator StartAnimClose()
    {
        _animator.Play("CloseNexo_Animation_Zone0");
        yield return new WaitForSeconds(3.5f);
        _showingMemory = false;

    }
    public void StartAnimBeat()
    {
        if (!_showingMemory)
        {
            _animator.SetBool("Beat", true);
        }
    }

    public IEnumerator StartMemory1()//ZONA1
    {
        StartAnimOpen();
        yield return new WaitForSeconds(2);
        _memorysAnims[0].Play("CharactersHappy_Animation_Story1");//FAMILIA FELIZ
        yield return new WaitForSeconds(2);
        StartCoroutine(StartAnimClose());
    }

    public IEnumerator StartMemory2()//ZONA1
    {
        StartAnimOpen();
        yield return new WaitForSeconds(2);
        _memorysAnims[1].Play("Mountain_Animation_Story1");
        yield return new WaitForSeconds(2);
        StartCoroutine(StartAnimClose());
    }
    
    public IEnumerator StartMemory3()//ZONA2
    {
        StartAnimOpen();
        yield return new WaitForSeconds(2);
        _memorysAnims[2].Play("Constelation_Animation_Story5");
        yield return new WaitForSeconds(0.2f);
        _memorysAnims[3].Play("RoomAssets_Animation_Story5");
        yield return new WaitForSeconds(2);
        StartCoroutine(StartAnimClose());
    }

    public IEnumerator StartMemory4()//ZONA2
    {
        StartAnimOpen();
        yield return new WaitForSeconds(2);
        _memorysAnims[4].Play("Gradas_Animation_Story3");
        yield return new WaitForSeconds(2);
        StartCoroutine(StartAnimClose());
    }

    public IEnumerator StartMemory5()//ZONA3
    {
        StartAnimOpen();
        yield return new WaitForSeconds(2);
        _memorysAnims[5].Play("VolcanCh_Animation_Story10");
        yield return new WaitForSeconds(2);
        StartCoroutine(StartAnimClose());
    }

    public IEnumerator StartMemory6()//ZONA3
    {
        StartAnimOpen();
        yield return new WaitForSeconds(2);
        _memorysAnims[6].Play("Volcan_Animation_Story10");
        yield return new WaitForSeconds(2);
        StartCoroutine(StartAnimClose());
    }
}

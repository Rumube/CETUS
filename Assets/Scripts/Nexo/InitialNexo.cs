using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialNexo : MonoBehaviour
{
    [SerializeField]private int _fragments = 0;
    [SerializeField]private Animator _animatorTitle;
    [SerializeField]private AnimationNexo _anim;

    private void Awake()
    {
        _anim = GetComponent<AnimationNexo>();
    }

    public void AddFragment()
    {
        _fragments++;
        if(_fragments==8)
        {
            _anim.StartAnimOpen();
            StartCoroutine(JumpNextScene());
        }
    }
    public IEnumerator JumpNextScene()
    {
        yield return new WaitForSeconds(4f);
        _animatorTitle.Play("Title_Animation");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
}

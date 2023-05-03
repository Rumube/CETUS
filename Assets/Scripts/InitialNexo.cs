using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialNexo : MonoBehaviour
{
    [SerializeField]private int _fragments = 0;
    [SerializeField]private Animator _animator;
    [SerializeField]private Animator _animatorTitle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFragment()
    {
        _fragments++;
        if(_fragments==8)
        {
            _animator.Play("Nexo_Animation01_Zone0");
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

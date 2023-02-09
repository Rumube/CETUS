using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limitation : MonoBehaviour
{
    private Transform nexo;
   
    public float[] _maxDistance;
    public int _level;
   

    private Animator _animator;
    // Start is called before the first frame update
    private void Awake()
    {
        nexo = GameObject.FindGameObjectWithTag("Nexo").GetComponent<Transform>();
    }
    void Start()
    {
      
        //_maxDistance[0] = nexo.transform.position;
        _animator = GetComponent<Animator>();
      
    }
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);

        for (int i = 0; i < _maxDistance.Length; i++)
        {
            
            Gizmos.DrawSphere(nexo.position, _maxDistance[i]);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(nexo.transform.position, transform.position)>=_maxDistance[_level-1])
        {
            print("return");
        }
    }
    
}

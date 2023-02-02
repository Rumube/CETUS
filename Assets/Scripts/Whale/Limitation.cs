using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limitation : MonoBehaviour
{
    private Transform nexo;
    [Tooltip("La primera posicion es la del nexo")]
    public Vector3[] _maxDistance;
    int _level;
   

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        nexo = GameObject.FindGameObjectWithTag("Nexo").GetComponent<Transform>();
        //_maxDistance[0] = nexo.transform.position;
        _animator = GetComponent<Animator>();
      
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _maxDistance.Length; i++)
        {
            if (i==0)
            {
                setBounderies(transform.position.x, nexo.position.x, _maxDistance[i].x, i + 1);
                setBounderies(transform.position.y, nexo.position.y, _maxDistance[i].y, i + 1);
                setBounderies(transform.position.z, nexo.position.z, _maxDistance[i].z, i + 1);
            }
            else
            {
                setBounderies(transform.position.x, _maxDistance[i -1].x, _maxDistance[i].x, i + 1);
                setBounderies(transform.position.y, _maxDistance[i - 1].y, _maxDistance[i].y, i + 1);
                setBounderies(transform.position.z, _maxDistance[i - 1].z, _maxDistance[i].z, i + 1);
            }
           

          
        }
       


        
    }
    public void setBounderies(float ballenaPos, float nexoPos, float maxDistance, int level)
    {
        if (Mathf.Abs(ballenaPos - nexoPos) <= maxDistance)
        {
            _level = level;
            Debug.Log("Nivel  " + level+ (ballenaPos - nexoPos));
        }
    }
}

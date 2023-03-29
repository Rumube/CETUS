using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Limitation : MonoBehaviour
{
    private Transform nexo;
   
    public float[] _maxDistance;
    public int _level;
    CinemachineFreeLook _cinemachine;
    WormManager _wormManager;
    PlayerController _playerController;
    public GameObject _wormhole;
    public Transform endWormhole;
    private Animator _animator;
    Vector3 prevPos;
    // Start is called before the first frame update
    private void Awake()
    {
        nexo = GameObject.FindGameObjectWithTag("Nexo").GetComponent<Transform>();
    }
    void Start()
    {
        _wormManager = GameObject.FindGameObjectWithTag("WormManager").GetComponentInChildren<WormManager>();
        _cinemachine = GameObject.FindGameObjectWithTag("FreeLook").GetComponent<CinemachineFreeLook>();
        _playerController = GetComponent<PlayerController>();
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
       
        if (Vector3.Distance(nexo.transform.position, transform.position)>=_maxDistance[_level-1] && _wormManager._outside == true)
        {
            print("Teletransporta worm");

            prevPos= transform.position;
           
                transform.position = _wormhole.transform.position;
                //_cinemachine.m_Lens.FieldOfView = Mathf.Lerp(40, 179, 5f);
                _wormManager._outside = false;
                _cinemachine.m_Lens.FieldOfView =179;
            _playerController.SetWhaleState(PlayerController.WHALE_STATE.wormhole);







        }
        else if (Vector3.Distance(endWormhole.position, transform.position) <= 5 && _wormManager._outside == false)
        {
            Debug.Log("Teletransporta al siguiente nivel " + new Vector3( _maxDistance[_level], prevPos.y,  _maxDistance[_level]) );
            transform.position = new Vector3( _maxDistance[_level], prevPos.y,  _maxDistance[_level]);
            _cinemachine.m_Lens.FieldOfView = 40;
            _level+=2;
             _playerController.SetWhaleState(PlayerController.WHALE_STATE.move);
            _wormManager._outside = true;

        }

    }
    
}

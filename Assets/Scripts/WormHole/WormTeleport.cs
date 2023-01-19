using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WormTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public Transform _holeLocation;

    WormTeleport _wormHole;
    public GameObject _destiny;

    
    CinemachineFreeLook _cinemachine;
    WormManager _wormManager;
    
    
    void Start()
    {
        _wormManager= GameObject.FindGameObjectWithTag("WormManager").GetComponentInChildren<WormManager>();
        _wormHole = GameObject.FindGameObjectWithTag("WormHole").GetComponentInChildren<WormTeleport>();
       _holeLocation = GameObject.FindGameObjectWithTag("WormHole").transform.GetChild(0).gameObject.transform;
       _cinemachine = GameObject.FindGameObjectWithTag("FreeLook").GetComponent<CinemachineFreeLook>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            //_whole=!_whole;
            collision.gameObject.transform.position = _holeLocation.position;
            StartCoroutine("Enable");
            
            if (_wormManager._outside==true)//entra eb el aagujero
            {

                Debug.Log("false");
                _cinemachine.m_Lens.FieldOfView = 179;
                _wormManager._outside = false;
            }
            else
            {
                Debug.Log("hola");
                _cinemachine.m_Lens.FieldOfView = 40;
                _wormManager._outside = true;
            }
            _wormHole._holeLocation= _destiny.transform;
        }
    }
    IEnumerator Enable()
    {
        _destiny.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(5f);
        _destiny.GetComponent<Collider>().enabled = true;
    }
}

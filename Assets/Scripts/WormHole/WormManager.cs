using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;


public class WormManager : MonoBehaviour
{
    [HideInInspector]
    public bool _outside = true;
    public PlayerController player;
   // CinemachineFreeLook _cinemachine;

    void Start()
    {
        //_cinemachine = GameObject.FindGameObjectWithTag("FreeLook").GetComponent<CinemachineFreeLook>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (_outside==false)
        //{
        //    player.SetWhaleState(PlayerController.WHALE_STATE.wormhole);
        //}
       
    }
    public void CameraChange()
    {

    }
}

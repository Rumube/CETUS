using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Limitation : MonoBehaviour
{
    public float[] maxDistance;
    public Transform[] centerOfTheLevel;
    public int _level;

    public CinemachineVirtualCamera _cinemachine;
    PlayerController _playerController;

    public GameObject wormhole;
    public Transform endWormhole;

    bool nextLevel;
    bool _outside=true;
    float timer = 0;
    // Start is called before the first frame update
  
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }
    

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);

        for (int i = 0; i < maxDistance.Length; i++)
        {
            
            Gizmos.DrawSphere(centerOfTheLevel[i].position, maxDistance[i]);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (_outside == true)
        {
            if (Vector3.Distance(centerOfTheLevel[_level - 1].position, transform.position) >= maxDistance[_level - 1] )
            {
                nextLevel = true;
                TeleportToWormHole();
            }
            else if (Vector3.Distance(centerOfTheLevel[_level - 1].position, transform.position) <= 10)
            {
                nextLevel = false;
                TeleportToWormHole();
            }
        }
       
        else if (Vector3.Distance(endWormhole.position, transform.position) <= 5 &&_outside == false)
        {
            if (nextLevel==true && _level != centerOfTheLevel.Length)
            {
                transform.position = new Vector3(centerOfTheLevel[_level].position.x+20, centerOfTheLevel[_level].position.y + 20, centerOfTheLevel[_level].position.z + 20);
                
                _level++;
            }
            else if (nextLevel == false || _level == centerOfTheLevel.Length)
            {
                _level--;
                transform.position = new Vector3(centerOfTheLevel[_level-1].position.x+20, centerOfTheLevel[_level-1].position.y + 20, centerOfTheLevel[_level-1].position.z + 20);
            }
            StartCoroutine(DesactivateCamera());
            // _cinemachine.m_Lens.FieldOfView = Mathf.Lerp(179, 40, timer);
            DowngradeFOV(179,40);
            _playerController.SetWhaleState(PlayerController.WHALE_STATE.move);
            _outside = true;

        }
    }
        IEnumerator DesactivateCamera()
    {
        yield return new WaitForSeconds(3f);
        _cinemachine.gameObject.SetActive(false);
    }
    IEnumerator UpgradeFOV(int initial, int finished)
    {
        while (_cinemachine.m_Lens.FieldOfView < finished)
        {
            _cinemachine.m_Lens.FieldOfView += 10;
            yield return new WaitForSeconds(0.25f);
        }
      
    }
    void DowngradeFOV(int initial, int finished)
    {
        if (_cinemachine.m_Lens.FieldOfView > finished)
        {
            _cinemachine.m_Lens.FieldOfView -= 10;
        }
           


    }
    void TeleportToWormHole()
    {
        transform.position = wormhole.transform.position;
        _outside = false;
        _cinemachine.gameObject.SetActive(true);
        StartCoroutine(UpgradeFOV(40,179));
       // _cinemachine.m_Lens.FieldOfView = Mathf.Lerp(40, 179, 0.25f);
        _playerController.SetWhaleState(PlayerController.WHALE_STATE.wormhole);
    }
    
}

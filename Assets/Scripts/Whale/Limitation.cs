using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using FMODUnity;
public class Limitation : MonoBehaviour
{
    [Header("Elementos por nivel")]
    [SerializeField] private GameObject[] _zones;

    public float[] maxDistance;
    public Transform[] centerOfTheLevel;
    public int _level;

    public CinemachineVirtualCamera _cinemachine;
    PlayerController _playerController;

    public GameObject wormhole;
    public Transform endWormhole;
    [SerializeField]
    List<Material> skyBoxes=new List<Material>();
    bool nextLevel;
    bool _outside = true;
    float timer = 0;
    // Start is called before the first frame update

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        StartCoroutine(WaitBoids());
    }
    private IEnumerator WaitBoids()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateLvl();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);

        for (int i = 0; i < maxDistance.Length; i++)
        {
            if (centerOfTheLevel[i] != null)
            {
                Gizmos.DrawSphere(centerOfTheLevel[i].position, maxDistance[i]);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (_outside == true)//entra
        {
            if (Vector3.Distance(centerOfTheLevel[_level - 1].position, transform.position) >= maxDistance[_level - 1])
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

        else if (Vector3.Distance(endWormhole.position, transform.position) <= 25 && _outside == false)
        {
            if (nextLevel == true && _level != centerOfTheLevel.Length)
            {
                transform.position = new Vector3(centerOfTheLevel[_level].position.x + 20, centerOfTheLevel[_level].position.y + 20, centerOfTheLevel[_level].position.z + 20);

                _level++;
                UpdateLvl();
            }
            else if (nextLevel == false || _level == centerOfTheLevel.Length)
            {
                _level--;
                UpdateLvl();
                transform.position = new Vector3(centerOfTheLevel[_level - 1].position.x + 20, centerOfTheLevel[_level - 1].position.y + 20, centerOfTheLevel[_level - 1].position.z + 20);
            }
            RenderSettings.skybox = skyBoxes[_level-1];
            StartCoroutine(DesactivateCamera(179, 40));
            //DowngradeFOV(179,40);
            _playerController.SetWhaleState(PlayerController.WHALE_STATE.move);
            _outside = true;

        }
    }

    private void UpdateLvl()
    {
        UpdateZones();
        if (_level == 1)
        {
            print("Level 1");
            if (!_zones[0].GetComponent<StudioEventEmitter>().IsPlaying())
            {
                _zones[1].GetComponent<StudioEventEmitter>().Stop();
                _zones[2].GetComponent<StudioEventEmitter>().Stop();
                _zones[0].GetComponent<StudioEventEmitter>().Play();
            }
        }
        else if (_level == 2)
        {
            print("Level 2");
            if (!_zones[1].GetComponent<StudioEventEmitter>().IsPlaying())
            {
                _zones[0].GetComponent<StudioEventEmitter>().Stop();
                _zones[2].GetComponent<StudioEventEmitter>().Stop();
                _zones[1].GetComponent<StudioEventEmitter>().Play();
            }
        }
        else if (_level == 3)
        {
            if (!_zones[2].GetComponent<StudioEventEmitter>().IsPlaying())
            {
                _zones[0].GetComponent<StudioEventEmitter>().Stop();
                _zones[1].GetComponent<StudioEventEmitter>().Stop();
                _zones[2].GetComponent<StudioEventEmitter>().Play();
            }
        }
    }

    private void UpdateZones()
    {
        foreach (GameObject zone in _zones)
        {
            zone.SetActive(false);
        }
        _zones[_level - 1].SetActive(true);
    }

    IEnumerator DesactivateCamera(int initial, int finished)
    {
        while (_cinemachine.m_Lens.FieldOfView > initial)
        {
            _cinemachine.m_Lens.FieldOfView -= 2;
            yield return new WaitForSeconds(0.01f);
        }
        _cinemachine.gameObject.SetActive(false);
    }
    IEnumerator UpgradeFOV(int initial, int finished)
    {
        while (_cinemachine.m_Lens.FieldOfView < finished)
        {
            _cinemachine.m_Lens.FieldOfView += 2;
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = wormhole.transform.position;
    }
    void TeleportToWormHole()
    {

        _outside = false;

        _cinemachine.gameObject.SetActive(true);
        StartCoroutine(UpgradeFOV(40, 179));
        _playerController.SetWhaleState(PlayerController.WHALE_STATE.wormhole);
    }

}

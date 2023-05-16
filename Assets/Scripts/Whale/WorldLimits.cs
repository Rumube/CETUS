using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLimits : MonoBehaviour
{
    [SerializeField] GameObject cetus;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float distanceLimitation;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(cetus.transform.position, spawnPoint.transform.position));
        if (Vector3.Distance(cetus.transform.position,spawnPoint.transform.position) >= distanceLimitation)
        {
            cetus.transform.rotation = spawnPoint.transform.rotation;
            cetus.transform.position = spawnPoint.transform.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerPosition : MonoBehaviour
{

    private GameObject player;
    private Material seaweedMat;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        seaweedMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.GetComponent<Transform>().position;
        seaweedMat.SetVector("_trackerPosition", playerPos);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerPosition : MonoBehaviour
{

    private GameObject player;

    private Material seaweedMat;

    // Start is called before the first frame update
    void Start()
    {
        seaweedMat = GetComponent<Renderer>().material;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.GetComponent<Transform>().position;

        seaweedMat.SetVector("trackerPosition", playerPos);
    }
}
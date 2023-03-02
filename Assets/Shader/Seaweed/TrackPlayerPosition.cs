using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayerPosition : MonoBehaviour
{

    private GameObject player;
    private GameObject pez;
    [SerializeField] private float _radius;
    private Material seaweedMat;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pez = GameObject.Find("Pez");

        seaweedMat = GetComponent<Renderer>().material;
        seaweedMat.SetFloat("_RadioAlgaJugador", player.GetComponent<Transform>().localScale.x - 0.4f);        
        seaweedMat.SetFloat("_RadioAlgaPez", pez.GetComponent<Transform>().localScale.x - 0.4f);
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 playerPos = player.GetComponent<Transform>().position;
        Vector3 pezPos = pez.GetComponent<Transform>().position;

        seaweedMat.SetVector("_trackerPosition", playerPos);
        seaweedMat.SetVector("_trackerPositionPez", pezPos);

        /* Collider[] coll = Physics.OverlapSphere(transform.position, _radius);
         List<Collider> collList = new List<Collider>(coll);
         List<Collider> finalColl = new List<Collider>();
         foreach (Collider currentCollider in collList)
         {
             if(currentCollider.tag == "Player" || currentCollider.tag == "Fish")
             {
                 finalColl.Add(currentCollider);
             }
         }*/

    }
}
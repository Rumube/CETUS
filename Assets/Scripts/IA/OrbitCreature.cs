using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCreature : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform orbit;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3.MoveTowards(transform.position, orbit.transform.position, speed*Time.deltaTime);
        transform.LookAt(orbit);
        transform.Translate(Vector3.forward * speed);
        // transform.rotation = Quaternion.Slerp(transform.rotation,orbit.transform.rotation, speed * Time.deltaTime);
        // Check if the position of the cube and sphere are approximately equal.
        //if (Vector3.Distance(transform.position, orbit.position) < 0.001f)
        //{
        //    // Swap the position of the cylinder.
        //    orbit.position *= -1.0f;
        //}
    }
}

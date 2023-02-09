using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject {
    // Settings
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float perceptionRadius = 8f;//2.5f
    public float avoidanceRadius = 10;//1
    public float maxSteerForce = 3;//3

    public float alignWeight = 1;
    public float cohesionWeight = 1;
    public float seperateWeight = 10;//1

    public float targetWeight = 1;//original 1

    [Header ("Collisions")]
    public LayerMask obstacleMask;
    public float boundsRadius = 1f;//.27
    public float avoidCollisionWeight = 50;//10
    public float collisionAvoidDst = 25;//5

}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    BoidSettings settings;
    private GameObject _player;
    private float _timeRunaway = 0;
    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;
    private bool _scared = false;
    // To update:
    Vector3 acceleration;
    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numPerceivedFlockmates;
    private GameObject _spawner;
    private float _maxRadius;
    // Cached
    Material material;
    Transform cachedTransform;
    Transform target;
    private Vector3 _lastDirect;

    void Awake()
    {
        material = transform.GetComponentInChildren<MeshRenderer>().material;
        _player = GameObject.FindGameObjectWithTag("Player");
        cachedTransform = transform;
    }
    /// <summary>
    /// Gets initial position of the boid
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="target"></param>
    public void Initialize(BoidSettings settings, Transform target)
    {
        this.target = target;
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        Vector3 random = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 2f), Random.Range(-1f, 2f));
        velocity = random * startSpeed;
    }

    public void SetColour(Color col)
    {
        if (material != null)
        {
            material.color = col;
        }
    }

    public void UpdateBoid(Transform Spawner)
    {
        if(Time.realtimeSinceStartup >= _timeRunaway)
        {
            Vector3 acceleration = Vector3.zero;
            _scared = ScaredController();
            if (target != null)
            {
                Vector3 offsetToTarget = (target.position - position);
                acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
            }

            if (numPerceivedFlockmates != 0)
            {
                centreOfFlockmates /= numPerceivedFlockmates;

                Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

                var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
                var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;

                acceleration += alignmentForce;
                acceleration += cohesionForce;
                acceleration += seperationForce;
            }

            if (_scared)
            {
                Vector3 direction = (position - _player.transform.position).normalized;
                _lastDirect = direction;
                Vector3 collisionAvoidForce = SteerTowards(direction) * settings.avoidCollisionWeight;
                acceleration += collisionAvoidForce * 10;
            }
            else if (IsHeadingForCollision())
            {
                Vector3 collisionAvoidDir = ObstacleRays();
                Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
                acceleration += collisionAvoidForce;
            }
            else if (OutofRadious())
            {
                Vector3 direction = (_spawner.transform.position - position).normalized;
                Vector3 collisionAvoidForce = SteerTowards(direction) * settings.avoidCollisionWeight;
                acceleration += collisionAvoidForce;
            }
            if (_scared)
            {
                velocity += acceleration * Time.deltaTime;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;
                velocity = dir * speed;

                cachedTransform.position += velocity * Time.deltaTime;
                cachedTransform.forward = dir;
                position = cachedTransform.position;
                forward = dir;
            }
            else
            {
                velocity += acceleration * Time.deltaTime;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;
                speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);

                velocity = dir * speed;
                cachedTransform.position += velocity * Time.deltaTime;
                cachedTransform.forward = dir;
                position = cachedTransform.position;
                forward = dir;
            }
        }
        else
        {
            //print("ENtro");
            //Vector3 collisionAvoidForce = SteerTowards(_lastDirect) * settings.avoidCollisionWeight;
            //acceleration += collisionAvoidForce;

            //velocity += acceleration * Time.deltaTime * 5;
            //float speed = velocity.magnitude;
            //Vector3 dir = velocity / speed;
            //velocity = dir * speed;

            //cachedTransform.position += velocity * Time.deltaTime;
            //cachedTransform.forward = dir;
            //position = cachedTransform.position;
            //forward = dir;
            transform.position += transform.forward * 0.5f;
        }
        
    }

    private bool OutofRadious()
    {
        float dist = Vector3.Distance(position, _spawner.transform.position);
        bool result = false;
        if (dist >= _maxRadius)
        {
            result = true;
        }
        return result;
    }

    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask))
        {
            return true;
        }
        else { }
        return false;
    }
    /// <summary>
    /// Detects if there is a obstacle
    /// </summary>
    /// <returns>If  detects an obstacle returns an alternative direction if not keep moving forward</returns>
    Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);//catches the direction of the rays
            Ray ray = new Ray(position, dir);
            if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask))
            {//
                return dir;
            }
        }

        return forward;
    }
    /// <summary>
    /// directs the boid to the indicated direction 
    /// </summary>
    /// <param name="vector"> the direction </param>
    /// <returns></returns>
    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }

    public void InitValues(GameObject spawner, float radius)
    {
        _spawner = spawner;
        _maxRadius = radius;
    }
    private bool ScaredController()
    {
        if (Vector3.Distance(_player.transform.position, position) <= _maxRadius/2)
        {
            _timeRunaway = Time.realtimeSinceStartup + 1f;
            return true;
        }
        else
        {
            return false;
        }
    }
}
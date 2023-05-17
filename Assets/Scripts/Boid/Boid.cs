using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class Boid : MonoBehaviour
{
    private Transform _spawnerParent;

    BoidSettings settings;
    private GameObject _player;
    private float _timeRunaway = 0;
    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    [SerializeField]private Vector3 velocity;
    private bool _isScared = false;
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
    [SerializeField] private FishMaterial _fishMaterial;

    enum behaviour { Scared, Follower };
    behaviour action;

    bool oneTime;

    void Awake()
    {
        if(transform.GetComponentInChildren<MeshRenderer>() != null)
        {
            material = transform.GetComponentInChildren<MeshRenderer>().material;
        }
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

        float startSpeed;
        if (action==behaviour.Follower)
        {
            startSpeed = (45 +60) / 2;
        }
        else
        {
           startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        }
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
    public void SetBehaviour(Spawner._behaviour behaviour)
    {
        action = (behaviour)behaviour;
    }

    public void UpdateBoid(Transform Spawner)
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= 1000)
        {
            if (Time.realtimeSinceStartup >= _timeRunaway)
            {
                acceleration = Vector3.zero;
                _isScared = PlayerDetection();
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
                    var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight * 10;

                    acceleration += alignmentForce;
                    acceleration += cohesionForce;
                    acceleration += seperationForce;
                }
                if (OutofRadious())
                {
                    Vector3 direction = (_spawner.transform.position - position).normalized;
                    Vector3 collisionAvoidForce = SteerTowards(direction) * settings.avoidCollisionWeight;
                    acceleration += collisionAvoidForce;
                }
                else if (action == behaviour.Follower)
                {

                    Vector3 direction = (_player.transform.position - position).normalized;

                    Vector3 cohesionWeight = SteerTowards(direction) * settings.cohesionWeight;
                    acceleration += cohesionWeight * 10;
                }
                else if (_isScared)
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


                velocity += acceleration * Time.deltaTime;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;

                if (action == behaviour.Follower)
                {

                    if (Vector3.Distance(position, _player.transform.position) <= 20)
                    {
                        speed = Mathf.Clamp(speed, 50, 60);
                    }
                    else
                    {
                        speed = Mathf.Clamp(speed, 60, 100);
                    }
                }
                else
                {
                    speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
                }
                velocity = dir * speed;
                cachedTransform.position += velocity * Time.deltaTime;
                cachedTransform.forward = dir;
                position = cachedTransform.position;
                forward = dir;
            }
            else
            {
                transform.position += transform.forward * 0.5f;
            }
        }
    }
    /// <summary>
    /// Checks if the boid is in or out of the spawn area
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Detects a collision
    /// </summary>
    /// <returns>true detects a collision an false if not</returns>
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
    /// Search a direction to avoid the obstacle
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
        Vector3 v;
        if (action==behaviour.Follower)
        {
             v = vector.normalized * 45 - velocity;//whale speed
        }
        else
        {
           v = vector.normalized * settings.maxSpeed - velocity;
        }
        
        return Vector3.ClampMagnitude(v, settings.maxSteerForce);
    }

    public void InitValues(GameObject spawner, float radius, Transform parent)
    {
        _spawner = spawner;
        _maxRadius = radius;
        _spawnerParent = parent;
        transform.SetParent(_spawnerParent);
    }
    private bool PlayerDetection()
    {
        if (Vector3.Distance(_player.transform.position, position) <= _maxRadius/2 && action==behaviour.Scared)
        {
            _timeRunaway = Time.realtimeSinceStartup + 1f;
            if(_fishMaterial != null)
            {
                _fishMaterial.SetFuerza(1f);
            }

            return true;
        }
        else
        {
            if(_fishMaterial != null)
            {
                _fishMaterial.SetFuerza(0.4f);
            }
            return false;
        }
    }
}
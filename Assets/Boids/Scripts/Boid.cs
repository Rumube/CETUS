using System.Collections;
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
    private bool _playerDetected = false;
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
    public void SetBehaviour(Spawner._behaviour behaviour)
    {
        action = (behaviour)behaviour;
    }

    public void UpdateBoid(Transform Spawner)
    {
        if (Time.realtimeSinceStartup >= _timeRunaway)
        {
            Vector3 acceleration = Vector3.zero;
            _playerDetected = PlayerDetection();
            if (target != null)
            {
                Vector3 offsetToTarget = (target.position - position);
                acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
            }
            //if (action == behaviour.Follower)
            //{
             
            //    Vector3 offset = _player.transform.position - position;
            //    float sqrDst = offset.x * offset.x + offset.y * offset.y + offset.z * offset.z;
               
            //    if (sqrDst < settings.perceptionRadius * 100)
            //    {
            //        print("seguir");
            //        if (oneTime == true)
            //        {
            //            numPerceivedFlockmates += 1;
            //            oneTime = false;
            //        }
                    
            //        avgFlockHeading += _player.transform.position.normalized;
            //        centreOfFlockmates += _player.transform.position;

            //        if (sqrDst < settings.avoidanceRadius * settings.avoidanceRadius)
            //        {
            //            avgAvoidanceHeading -= offset / sqrDst;
            //        }
            //    }
            //    else
            //    {
            //        oneTime = true;
            //    }
            //    var playerAlignmentForce = SteerTowards(_player.transform.position.normalized) * settings.alignWeight;
            //    acceleration += playerAlignmentForce;
            //    Vector3 _playerCentre = (_player.transform.position - position);
            //    var cohesionForce = SteerTowards(_playerCentre) * settings.cohesionWeight;
            //}
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
            if (action == behaviour.Follower)
            {
                Debug.Log(action);
                Vector3 direction = (_player.transform.position - position).normalized;

                Vector3 collisionAvoidForce = SteerTowards(direction) * settings.cohesionWeight;
                acceleration += collisionAvoidForce;

               
            }
             else if (_playerDetected)
            {
                if (action==behaviour.Scared)
                {
                    Vector3 direction = (position - _player.transform.position).normalized;
                    _lastDirect = direction;
                    Vector3 collisionAvoidForce = SteerTowards(direction) * settings.avoidCollisionWeight;
                    acceleration += collisionAvoidForce * 10;
                }
                //else if (action == behaviour.Follower)
                //{
                //    Debug.Log(action);
                //    Vector3 direction = ( _player.transform.position-position).normalized;

                //    Vector3 collisionAvoidForce = SteerTowards(direction) * settings.cohesionWeight;
                //    acceleration += collisionAvoidForce*10;
                //}
                
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

            if (_playerDetected)
            {
                if (action == behaviour.Scared)
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
               

            }
            if (action == behaviour.Follower)
            {
                velocity += acceleration * Time.deltaTime;
                float speed = velocity.magnitude;
                Vector3 dir = velocity / speed;

                //Debug.Log("actionfollower"+_player.transform.position);
                if (Vector3.Distance(position, _player.transform.position) <= 10)
                {
                    speed = 1;
                    velocity = dir * speed;
                }
                else
                {
                    speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
                    velocity = dir * speed;
                }
                
                cachedTransform.position += velocity * Time.deltaTime;
                cachedTransform.forward = dir;
                position = cachedTransform.position;
                forward = dir;
                //velocity += acceleration * Time.deltaTime;
                //float speed = velocity.magnitude;
                //Vector3 dir = velocity / speed;
                //if (Vector3.Distance(_player.transform.position, position)<=_maxRadius/3)
                //{
                //    dir = _player.transform.position.normalized;
                //    velocity = dir * 5;
                //}
                //else
                //{
                //    velocity = dir * speed;
                //}


                //cachedTransform.position += velocity * Time.deltaTime;
                //cachedTransform.forward = dir;
                //position = cachedTransform.position;
                //forward = dir;
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
            Debug.Log("forward");
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
    private bool PlayerDetection()
    {
        if (Vector3.Distance(_player.transform.position, position) <= _maxRadius/2 && action==behaviour.Scared)
        {
            _timeRunaway = Time.realtimeSinceStartup + 1f;
            _fishMaterial.SetFuerza(1f);
            return true;
        }
        else
        {
            _fishMaterial.SetFuerza(0.4f);
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    //References
    private GameObject _nexo;

    //Parameters
    [Header("Parameters")]
    public float _followVelocity;
    private Vector3 _target;
    private GameObject _whale;
    private SphereCollider _collider;
    private Transform[] _posibleFollows = new Transform[3];
    private Transform _followTarget;

    public enum MemoryState
    {
        followNexo,
        followWhave,
        followTail
    }

    public MemoryState _memoryState;
    [SerializeField] private CollectedMemory.Zone _zone;

    // Start is called before the first frame update
    void Start()
    {
        _whale = GameObject.FindGameObjectWithTag("Player");
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = true;
        _memoryState = MemoryState.followWhave;
        SelectNewTarget();
        _nexo = GameObject.FindGameObjectWithTag("Nexo");
    }

    // Update is called once per frame
    void Update()
    {
        FollowMovement();

        if (_memoryState == MemoryState.followNexo && transform.position == _target)
        {
            _nexo.GetComponent<AnimationNexo>().StartAnimBeat();
            if (_nexo.GetComponent<InitialNexo>() != null)
            {
                _nexo.GetComponent<InitialNexo>().AddFragment();
            }else if(_nexo.GetComponent<NexoManager>() != null)
            {
                _nexo.GetComponent<NexoManager>().FragmentProvided(_zone);
            }
            Destroy(this.gameObject);
        }
    }

    void SelectNewTarget()
    {
        _posibleFollows[0] = GameObject.FindGameObjectWithTag("AletaIz").transform;
        _posibleFollows[1] = GameObject.FindGameObjectWithTag("AletaDer").transform;
        _posibleFollows[2] = GameObject.FindGameObjectWithTag("Cola").transform;
        _followTarget = _posibleFollows[Random.Range(0, _posibleFollows.Length)];
    }

    public void SetZone(CollectedMemory.Zone zone)
    {
        _zone = zone;
    }
    /// <summary>
    /// Moves the memory in the correct direction
    /// </summary>
    private void FollowMovement()
    {
        switch (_memoryState)
        {
            case MemoryState.followNexo:
                transform.position = Vector3.MoveTowards(transform.position, _target, _followVelocity * Time.deltaTime);
                break;
            case MemoryState.followWhave:
                if(_followTarget == null)
                {
                    SelectNewTarget();
                }
                transform.position = Vector3.MoveTowards(transform.position, _followTarget.position, _followVelocity * Time.deltaTime);
                break;
            case MemoryState.followTail:
                transform.position = Vector3.MoveTowards(transform.position, _whale.transform.position, _followVelocity * Time.deltaTime);
                break;
            default:
                break;
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(_memoryState != MemoryState.followTail)
    //    {
    //        _memoryState = MemoryState.followNexo;
    //    }
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (_memoryState != MemoryState.followTail)
    //    {
    //        _memoryState = MemoryState.followWhave;
    //    }
    //}

    /// <summary>
    /// Set the base's position
    /// </summary>
    /// <param name="basePosition"></param>
    public void SetTarge(Vector3 basePosition)
    {
        _target = basePosition;
    }

    public void ChangeMemoryState(bool value)
    {

    }

    /// <summary>
    /// Set the momery state
    /// </summary>
    /// <param name="newState"></param>
    public void SetMemoryState(MemoryState newState)
    {
        _memoryState = newState;
        UpdateCollision();
    }

    /// <summary>
    /// Update the collisions and detect the memory position
    /// </summary>
    private void UpdateCollision()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }


}

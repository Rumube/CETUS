using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float _speedMove;
    public float _speedRotation;
    Vector3 _randomVector;
    bool _spin = true;
    public bool _wander = true;


    public int _rotationX;
    public int _rotationY;
    public int _rotationZ;

    public float _spinTime;//tiempo que tarda en volver a girar
    // Start is called before the first frame update
    void Start()
    {
        _randomVector = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2, 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (_wander == true)
        {
            transform.Rotate(_randomVector * _speedRotation * Time.deltaTime);
            transform.position += (transform.forward * _speedMove * Time.deltaTime);

            transform.Rotate(_randomVector * _speedRotation * Time.deltaTime);
            if (_spin == true)
            {
                _randomVector = new Vector3(Random.Range(-_rotationX, _rotationX + 1), Random.Range(-_rotationY, _rotationY + 1), Random.Range(-_rotationZ, _rotationZ + 1));

                StartCoroutine("SpinTime");
                _spin = false;


            }
        }
        else
        {
            transform.position += (transform.forward * _speedMove * Time.deltaTime);
            if (_randomVector == Vector3.zero)
            {
                transform.Rotate(new Vector3(_rotationX, _rotationY, _rotationZ) * _speedMove * Time.deltaTime);
            }
            else
            {
                transform.Rotate(-_randomVector * (_speedRotation + _speedMove) * Time.deltaTime);

            }

        }


    }
    void FixedUpdate()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(transform.position, Vector3.forward, out hit, 100.0f))
        //{
        //    print("Found an object - distance: " + hit.distance);
        //    _wander = false;
        //     Debug.DrawRay(transform.position, Vector3.forward, Color.green, 100f);
        //}
        //else
        //{
        //    _wander = true;
        //}


    }
    void Spin()
    {

    }
    IEnumerator SpinTime()
    {
        yield return new WaitForSeconds(_spinTime);
        _spin = true;

    }
    private void OnCollisionEnter(Collision collision)
    {
        _wander = false;
        Debug.Log("collision");
    }
    private void OnCollisionStay(Collision collision)
    {
        _wander = false;

        Debug.Log("collision");
    }
    private void OnCollisionExit(Collision collision)
    {
        _wander = true;
    }
    private void OnTriggerEnter(Collider other)
    {

        _wander = false;
    }
    private void OnTriggerStay(Collider other)
    {
        _wander = false;
    }
    private void OnTriggerExit(Collider other)
    {
        _wander = true;
    }

}
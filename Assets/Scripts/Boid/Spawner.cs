using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public enum GizmoType { Never, SelectedOnly, Always }
    public enum _behaviour { Scared, Follower };

    public Boid prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public Color colour;
    public GizmoType showSpawnRegion;
    public _behaviour behaviour;
    [SerializeField] private StudioEventEmitter _runSound;
    [SerializeField] [Range(0, 100)] private float _bubbleSoundProbability;

    void Awake () {
        for (int i = 0; i < spawnCount; i++) {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate (prefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;
            boid.InitValues(gameObject, spawnRadius);
            boid.SetBehaviour(behaviour);
            boid.SetColour (colour);
        }
        _runSound.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Random.Range(0, 100) < _bubbleSoundProbability && other.tag == "Player")
        {
            _runSound.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _runSound.Stop();
    }

    private void OnDrawGizmos () {
        if (showSpawnRegion == GizmoType.Always) {
            DrawGizmos ();
        }
    }

    void OnDrawGizmosSelected () {
        if (showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos ();
        }
    }

    void DrawGizmos () {

        Gizmos.color = new Color (colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawSphere (transform.position, spawnRadius);
    }

}
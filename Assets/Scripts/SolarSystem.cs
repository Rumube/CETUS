using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : SpaceObject
{
    public enum StarType
    {
        Azul,
        Blanco,
        Amarillo,
        Naranja,
        Roja
    }
    [Header("Stats")]
    public StarType _starType;
    public int _numPlanets;
    public List<GameObject> _StarParticles;

    #region References
    [Header("References")]
    public List<GameObject> _planets  = new List<GameObject>();
    public List<GameObject> _planetPositions = new List<GameObject>();
    public List<Material> _starMaterials = new List<Material>();
    public GameObject _orbit;
    System.Random _rng;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitValues()
    {
        _numPlanets = _rng.Next(1, 5);
        _starType = (StarType)_rng.Next(0,5);
        GetComponent<Renderer>().material = _starMaterials[(int)_starType];
        for (int i = 0; i < _numPlanets; i++)
        {
            int randomIndex = _rng.Next(0, _planets.Count);
            GameObject newPlanet = Instantiate(_planets[randomIndex], transform);
            //newPlanet.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            newPlanet.transform.position = _planetPositions[i].transform.position;
        }

        float angx = _rng.Next(0, 360);
        float angy = _rng.Next(0, 360);
        float angz = _rng.Next(0, 360);

        _orbit.transform.rotation = Quaternion.Euler(angx, angy, angz);

    }

    public void SetRng(System.Random value)
    {
        _rng = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject space = GameObject.FindGameObjectWithTag("Space");
            GameObject newParticle;
            switch (_starType)
            {
                case StarType.Azul:
                    newParticle = Instantiate(_StarParticles[0], space.transform);
                    break;
                case StarType.Blanco:
                    newParticle = Instantiate(_StarParticles[1], space.transform);
                    break;
                case StarType.Amarillo:
                    newParticle = Instantiate(_StarParticles[2], space.transform);
                    break;
                case StarType.Naranja:
                    newParticle = Instantiate(_StarParticles[3], space.transform);
                    break;
                case StarType.Roja:
                    newParticle = Instantiate(_StarParticles[4], space.transform);
                    break;
                default:
                    newParticle = Instantiate(_StarParticles[0], space.transform);
                    break;
            }

            newParticle.transform.position = transform.position;

            Destroy(gameObject);
        }
    }
}

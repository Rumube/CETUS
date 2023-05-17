using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Compass : MonoBehaviour
{
    [Header("Emision")]
    [SerializeField] private float _emissionIntensity = 1.2f;
    [SerializeField] private float _emissiveIntensity = 1;
    [SerializeField] private float _maxEmissionIntensity = 0;
    private Color _emissionColor = new Color(0.202095553f, 0.689502418f, 2.27060294f, 1);
    private Color _currentEmissionColor = new Color(0.202095553f, 0.689502418f, 2.27060294f, 1);

    [Header("Configuration")]
    public bool _traspassingToNexoLocked;

    [Header("Stats")]
    public int _currentMemories;
    public bool _compasActive;

    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer _armorMaterial;
    public List<GameObject> _memoryParticle = new List<GameObject>();
    private List<GameObject> _memoriesList = new List<GameObject>();
    private GameObject _nexo;

    private void Awake()
    {
        _nexo = GameObject.FindGameObjectWithTag("Nexo");
    }

    private IEnumerator UpdateLight()
    {
        do
        {
            yield return new WaitForSeconds(0.05f);
            _emissiveIntensity -= 1f;
            UpdateEmission();
        } while (_emissiveIntensity > 1);
        _emissiveIntensity = 1;
        UpdateEmission();
    }

    private void UpdateEmission()
    {
        if( _emissiveIntensity > _maxEmissionIntensity)
        {
            _emissiveIntensity = _maxEmissionIntensity;
        }
        _currentEmissionColor = _emissionColor * _emissiveIntensity;
        _currentEmissionColor = Color.Lerp(_currentEmissionColor, _emissionColor, 2f * Time.deltaTime);
        _armorMaterial.materials[0].SetColor("_EmissionColor", _currentEmissionColor);
    }

    /// <summary>
    /// Creates a new memory
    /// </summary>
    public void MemoriesUp(CollectedMemory.Zone zone)
    {
        _currentMemories++;

        int randomParticle = Random.Range(0, _memoryParticle.Count);
        GameObject newMemory = Instantiate(_memoryParticle[randomParticle], transform);
        newMemory.GetComponent<Memory>().SetTarge(_nexo.transform.position);
        newMemory.GetComponent<Memory>().SetZone(zone);
        _memoriesList.Add(newMemory);
        GenerateArmorEffect();
    }

    private void GenerateArmorEffect()
    {
        if(_emissiveIntensity < 200f)
        {
            _emissiveIntensity += _emissionIntensity;
            StopAllCoroutines();
            StartCoroutine(UpdateLight());
        }
    }

    /// <summary>
    /// Lose 1 memory
    /// </summary>
    public void MemoriesDown()
    {
        _currentMemories--;
    }

    /// <summary>
    /// Active or disactive the compass
    /// </summary>
    public void ChangeMemoryStates()
    {
        _compasActive = !_compasActive;
        foreach (GameObject memory in _memoriesList)
        {
            if (_compasActive)
            {
                memory.GetComponent<Memory>().SetMemoryState(Memory.MemoryState.followWhave);
            }
            else
            {
                memory.GetComponent<Memory>().SetMemoryState(Memory.MemoryState.followTail);
            }
        }
    }

    /// <summary>
    /// It is called when the whale touches the nexo
    /// </summary>
    public IEnumerator LeaveMemoriesIntoNexo()
    {
        _traspassingToNexoLocked = true;
        List<GameObject> _memoriesListAux = new List<GameObject>(_memoriesList);
        foreach (GameObject memory in _memoriesList)
        {
            GetComponent<StudioEventEmitter>().Play();
            
            memory.transform.SetParent(_nexo.transform);
            MemoriesDown();
            yield return new WaitForSeconds(0.5f);
            memory.GetComponent<Memory>()._memoryState = Memory.MemoryState.followNexo;
        }
        _memoriesList.Clear();
        _memoriesListAux.Clear();
        _traspassingToNexoLocked = false;
        transform.GetComponentInParent<WhalePahtController>().ActivateExitPath();
    }
}

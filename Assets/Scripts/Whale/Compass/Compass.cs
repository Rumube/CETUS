using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [Header("Configuration")]
    public bool _traspassingToNexoLocked;
    [Header("Stats")]
    public int _currentMemories;
    public bool _compasActive;

    [Header("References")]
    public List<GameObject> _memoryParticle = new List<GameObject>();
    private List<GameObject> _memoriesList = new List<GameObject>();
    private GameObject _nexo;

    private void Awake()
    {
        _nexo = GameObject.FindGameObjectWithTag("Nexo");
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
            memory.transform.SetParent(_nexo.transform);
            MemoriesDown();
            yield return new WaitForSeconds(0.5f);
            memory.GetComponent<Memory>()._memoryState = Memory.MemoryState.followNexo;
        }
        _memoriesList.Clear();
        _memoriesListAux.Clear();
        _traspassingToNexoLocked = false;
    }
}

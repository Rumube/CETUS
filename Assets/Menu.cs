using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    private GameObject _player;
    private PlayerController _playerController;
    private PlayerInputActions _inputActions;

    [Header("Options")]
    public GameObject Options;
    public GameObject OptionsSound;
    public GameObject OptionsControls;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _inputActions = _playerController.GetPlayerInputActions();
    }

    void Start()
    {
        Options.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Options.activeSelf == true)
            {
                Options.SetActive(false);
            }
        }
    }
    public void GeneralOptions()
    {
        Options.SetActive(true);
    }
    public void OptionsControlsMenu()
    {
        OptionsControls.SetActive(true);
        OptionsSound.SetActive(false);
    }
    public void OptionsSoundMenu()
    {
        OptionsControls.SetActive(false);
        OptionsSound.SetActive(true);
    }
    public void PlayScene()
    {
        //loadscene
    }
}

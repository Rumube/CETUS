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

    private float _nextPress = 0;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _inputActions = _playerController.GetPlayerInputActions();
        if (_inputActions == null)
        {
            _inputActions = _playerController.GetPlayerInputActions();
        }
    }

    void Start()
    {
        Options.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if((_inputActions.Gameplay.Menu.ReadValue<float>() != 0 || _inputActions.Paths.Menu.ReadValue<float>() != 0) && Time.realtimeSinceStartup >= _nextPress)
        {
            _nextPress = Time.realtimeSinceStartup + 1f;
            _player.GetComponent<PlayerController>().SetPause();
            if (Options.activeSelf)
            {
                Options.SetActive(false);
            }
            else
            {
                Options.SetActive(true);
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
        _player.GetComponent<PlayerController>().SetPause();
    }
}

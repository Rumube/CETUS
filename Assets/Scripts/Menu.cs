using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Vaues")]
    [SerializeField] private float _generalVolume = 0.5f;
    [SerializeField] private float _ambientVolume = 0.5f;
    [SerializeField] private float _musicVolume = 0.5f;

    private float _nextPress = 0;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }
    private void Start()
    {
        _inputActions = _playerController.GetPlayerInputActions();
        if (_inputActions == null)
        {
            _inputActions = _playerController.GetPlayerInputActions();
        }
        _playerController.SetPause();
    }

    // Update is called once per frame
    void Update()
    {
        if((_inputActions.Gameplay.Menu.ReadValue<float>() != 0 || _inputActions.Paths.Menu.ReadValue<float>() != 0) && Time.realtimeSinceStartup >= _nextPress)
        {
            _nextPress = Time.realtimeSinceStartup + 1f;
            _playerController.SetPause();
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
        _playerController.SetPause();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #region Controllers
    public void InvertMovement(Toggle toggle)
    {
        if(_playerController != null)
        {
            if (toggle.isOn)
            {
                _playerController.SetInvertValue(1);
            }
            else
            {
                _playerController.SetInvertValue(-1);
            }
        }
    }

    public void InvertCamera(Toggle toggle)
    {
        if(toggle.isOn)
        {
            //TODO: CAMARA INVERTIDA
        }
        else
        {
            //TODO: CAMARA NORMAL
        }
    }
    #endregion
    #region Sound
    public void GeneralSlider(Slider slider)
    {
        _generalVolume = slider.value;
    }

    public void AmbientSlider(Slider slider)
    {
        _ambientVolume = slider.value;
    }

    public void MusicSlider(Slider slider)
    {
        _musicVolume = slider.value;
    }
    #endregion
}

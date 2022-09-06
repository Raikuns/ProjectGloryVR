using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UniversalRenderPipelineAsset _pipeline;

    [Header("UI Stuff")]
    [SerializeField] private Slider _renderScale;
    [SerializeField] private Slider _masterAudio;
    [SerializeField] private Slider _vfxAudio;

    [Header("Settings Menu")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _howTo;

    [Header("Audio Mixers")]
    [SerializeField] private AudioMixerGroup _masterMix;
    [SerializeField] private AudioMixerGroup _vfxMix;

    private void Start()
    {
        _mainMenu.SetActive(true);
        _settingsPanel.SetActive(false);
        _howTo.SetActive(false);    
    }
    public void StartGame()
    {
        SceneManager.UnloadSceneAsync(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Settings()
    {
        _mainMenu.SetActive(false);
        _settingsPanel.SetActive(true);
    }
    public void BackButton()
    {
        if (_settingsPanel)
        {
            _settingsPanel.SetActive(false);
            _mainMenu.SetActive(true);
        }
        if (_howTo)
        {
            _howTo.SetActive(false);
            _mainMenu?.SetActive(true);    
        }
    }
    public void HowToPlay()
    {
        _mainMenu.SetActive(false);
        _howTo.SetActive(true);
    }

    public void ResolutionScale()
    {
        _pipeline.renderScale = _renderScale.value;
    }

    public void MasterAudio()
    {
        if (_masterAudio != null)
            _masterMix.audioMixer.SetFloat("master", _masterAudio.value);
    }
    public void VFXAudio()
    {
        if (_vfxMix != null)
            _vfxMix.audioMixer.SetFloat("vfx", _vfxAudio.value);
    }
}

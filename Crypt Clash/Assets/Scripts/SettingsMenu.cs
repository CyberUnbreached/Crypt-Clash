using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu Instance { get; private set; }

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Volume postProcessingVolume;

    private ColorAdjustments colorAdjustments;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;
    private int currentResolutionIndex;
    private bool isFullscreen;
    private bool vSyncOn;
    private float brightness;
    private float contrast;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider contrastSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    private Resolution[] resolutions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            List<string> resolutionOptions = new List<string>();
            foreach (var res in resolutions)
            {
                resolutionOptions.Add(res.width + " x " + res.height);
            }
            resolutionDropdown.AddOptions(resolutionOptions);
        }

        LoadSettings();

        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out ColorAdjustments ca))
        {
            colorAdjustments = ca;
        }

        ApplyAllSettings();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void setMasterVolume(float volume)
    {
        masterVolume = volume;
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", volume);
        }
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void setMusicVolume(float volume)
    {
        musicVolume = volume;
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", volume);
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void setSFXVolume(float volume)
    {
        sfxVolume = volume;
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFXVolume", volume);
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void setFullscreen(bool fullscreen)
    {
        isFullscreen = fullscreen;
        Screen.fullScreen = fullscreen;
        PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);
    }

    public void setResolution(int index)
    {
        currentResolutionIndex = index;
        if (resolutions != null && index >= 0 && index < resolutions.Length)
        {
            Resolution res = resolutions[index];
            Screen.SetResolution(res.width, res.height, isFullscreen);
            PlayerPrefs.SetInt("ResolutionIndex", index);
        }
    }

    public void SetVSync(bool isOn)
    {
        vSyncOn = isOn;
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
    }

    public void SetBrightness(float value)
    {
        brightness = value;
        PlayerPrefs.SetFloat("Brightness", value);
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = value;
        }
    }

    public void SetContrast(float value)
    {
        contrast = value;
        PlayerPrefs.SetFloat("Contrast", value);
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.contrast.value = value;
        }
    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
    }

    public void ApplyAllSettings()
    {
        LoadSettings();

        setMasterVolume(masterVolume);
        setMusicVolume(musicVolume);
        setSFXVolume(sfxVolume);
        SetVSync(vSyncOn);
        setFullscreen(isFullscreen);
        setResolution(currentResolutionIndex);
        SetBrightness(brightness);
        SetContrast(contrast);

        UpdateUIControls();

        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0f);
        brightness = PlayerPrefs.GetFloat("Brightness", 0f);
        contrast = PlayerPrefs.GetFloat("Contrast", 0f);
        isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        vSyncOn = PlayerPrefs.GetInt("VSync", 1) == 1;
        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
    }

    private void UpdateUIControls()
    {
        if (masterVolumeSlider) masterVolumeSlider.value = masterVolume;
        if (musicVolumeSlider) musicVolumeSlider.value = musicVolume;
        if (sfxVolumeSlider) sfxVolumeSlider.value = sfxVolume;
        if (brightnessSlider) brightnessSlider.value = brightness;
        if (contrastSlider) contrastSlider.value = contrast;
        if (fullscreenToggle) fullscreenToggle.isOn = isFullscreen;
        if (vsyncToggle) vsyncToggle.isOn = vSyncOn;

        if (resolutionDropdown && resolutions != null && currentResolutionIndex < resolutions.Length)
        {
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    public void UpdateSceneReferences(Volume newVolume)
    {
        postProcessingVolume = newVolume;

        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out ColorAdjustments ca))
        {
            colorAdjustments = ca;
            ApplyAllSettings();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (postProcessingVolume == null)
        {
            Volume foundVolume = GameObject.Find("PostProcessingVolume")?.GetComponent<Volume>();
            if (foundVolume != null)
            {
                UpdateSceneReferences(foundVolume);
            }
        }

        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out ColorAdjustments ca))
        {
            colorAdjustments = ca;
            ApplyAllSettings();
        }
        else
        {
            Debug.LogWarning("[SettingsMenu] Cannot apply settings: PostProcessingVolume or ColorAdjustments not found.");
        }
    }

    private void EnsureResolutionsInitialized()
    {
        if (resolutions == null || resolutions.Length == 0)
        {
            resolutions = Screen.resolutions;

            if (resolutionDropdown != null)
            {
                resolutionDropdown.ClearOptions();
                List<string> resolutionOptions = new List<string>();
                foreach (var res in resolutions)
                {
                    resolutionOptions.Add(res.width + " x " + res.height);
                }
                resolutionDropdown.AddOptions(resolutionOptions);
            }
        }
    }
}

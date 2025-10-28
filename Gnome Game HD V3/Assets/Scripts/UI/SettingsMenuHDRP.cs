using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SettingsMenuHDRP : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider audioSlider;
    [SerializeField] private Slider brightnessSlider;

    [Header("Post-Processing")]
    [SerializeField] private Volume globalVolume;

    private Exposure exposure;

    private void Awake()
    {
        // Make settings persistent across scenes (optional)
        DontDestroyOnLoad(gameObject);

        // Get exposure override from Volume
        if (globalVolume != null)
            globalVolume.profile.TryGet(out exposure);
    }

    private void Start()
    {
        // --- AUDIO ---
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        audioSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        audioSlider.onValueChanged.AddListener(SetVolume);

        // --- BRIGHTNESS ---
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    private void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    private void SetBrightness(float value)
    {
        if (exposure != null)
        {
            // Exposure compensation ranges roughly from -5 (dark) to +5 (bright)
            exposure.compensation.value = Mathf.Lerp(-2f, 2f, value);
        }

        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }
}

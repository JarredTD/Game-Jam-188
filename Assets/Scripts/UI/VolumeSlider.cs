using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string exposedParameter = "MasterVolume";

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void Start()
    {
        if (audioMixer.GetFloat(exposedParameter, out float currentValue))
        {
            slider.value = Mathf.InverseLerp(-80f, 0f, currentValue);
        }
    }

    private void HandleSliderValueChanged(float value)
    {
        float dbValue = Mathf.Lerp(-80f, 0f, value);
        audioMixer.SetFloat(exposedParameter, dbValue);

        PlayerPrefs.SetFloat("volume", value);
    }
}

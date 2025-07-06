using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();

        slider.minValue = 0.0001f;
        slider.maxValue = 1f;

        InitializeSlider();
    }

    private void InitializeSlider()
    {
        slider.value = AudioManager.Instance.GetMasterVolume();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderChanged);
    }
}
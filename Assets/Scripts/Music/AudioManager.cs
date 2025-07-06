using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

[DefaultExecutionOrder(-100)]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Settings")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioMixerGroup musicOutputGroup;
    [SerializeField][Range(0.1f, 3f)] private float musicFadeInDuration = 1.5f;

    [Header("Volume Settings")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private string volumeParameter = "MasterVolume";

    private AudioSource musicSource;
    private float currentVolume = 1f;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void Initialize()
    {
        CreateMusicSource();
        SetMasterVolume(currentVolume);
        StartMusicWithFade();
    }

    private void CreateMusicSource()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = menuMusic;
        musicSource.outputAudioMixerGroup = musicOutputGroup;
        musicSource.loop = true;
    }

    private void StartMusicWithFade()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusicIn());
    }

    private IEnumerator FadeMusicIn()
    {
        musicSource.volume = 0f;
        musicSource.Play();

        float timer = 0f;
        while (timer < musicFadeInDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.SmoothStep(0f, currentVolume, timer / musicFadeInDuration);
            yield return null;
        }

        musicSource.volume = currentVolume;
    }

    public void SetMasterVolume(float volume)
    {
        currentVolume = Mathf.Clamp01(volume);
        masterMixer.SetFloat(volumeParameter, ConvertToDecibels(currentVolume));

        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.volume = currentVolume;
        }
    }

    private float ConvertToDecibels(float volume)
    {
        return volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
    }

    public float GetMasterVolume() => currentVolume;
}
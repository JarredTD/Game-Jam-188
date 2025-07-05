using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float fullBlackDuration = 0.1f;

    [Header("Scene Event")]
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;

    [Header("Optional Blocking Control")]
    [SerializeField] private CanvasGroup canvasGroup;

    private static SceneTransitionController _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeFader();
    }
    private void InitializeFader()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    private void OnEnable() => sceneLoadEvent.OnRequestSceneLoad += HandleSceneLoad;
    private void OnDisable() => sceneLoadEvent.OnRequestSceneLoad -= HandleSceneLoad;

    private void HandleSceneLoad(SceneLoaderSO loader)
    {
        if (!string.IsNullOrEmpty(loader.sceneName))
            StartCoroutine(FadeAndLoad(loader.sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return Fade(0f, 1f);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
            yield return null;

        loadOp.allowSceneActivation = true;

        yield return null;
        yield return null;

        yield return new WaitForSecondsRealtime(fullBlackDuration);

        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float start, float end)
    {
        if (end < start)
        {
            Time.timeScale = 1f;
            yield return null;
        }
        else
        {
            Time.timeScale = 0f;
        }

        float time = 0f;
        Color color = fadeImage.color;

        if (canvasGroup != null && end > start)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = false;
        }

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(start, end, time / fadeDuration);
            fadeImage.color = color;

            if (canvasGroup != null)
                canvasGroup.alpha = color.a;

            yield return null;
        }

        color.a = end;
        fadeImage.color = color;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = end;
            canvasGroup.blocksRaycasts = end > 0f;
            canvasGroup.interactable = false;
        }

        if (start > end)
            Time.timeScale = 1f;
    }
}
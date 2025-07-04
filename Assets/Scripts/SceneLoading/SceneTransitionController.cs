using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Scene Event")]
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;

    [Header("Optional Blocking Control")]
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    private void OnEnable()
    {
        if (sceneLoadEvent != null)
        {
            Debug.Log("SceneTransitionController subscribing to event");
            sceneLoadEvent.OnRequestSceneLoad += HandleSceneLoad;
        }
    }

    private void OnDisable()
    {
        if (sceneLoadEvent != null)
        {
            Debug.Log("SceneTransitionController unsubscribing from event");
            sceneLoadEvent.OnRequestSceneLoad -= HandleSceneLoad;
        }
    }

    private void HandleSceneLoad(SceneLoaderSO loader)
    {
        Debug.Log($"SceneTransitionController received a request");
        if (!string.IsNullOrEmpty(loader.sceneName))
            Debug.Log($"Scene name: {loader.sceneName}");
        StartCoroutine(FadeAndLoad(loader.sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return Fade(0f, 1f);
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float start, float end)
    {
        float time = 0f;
        Color color = fadeImage.color;

        if (canvasGroup != null && end > start)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = false;
        }

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            float alpha = Mathf.Lerp(start, end, t);

            color.a = alpha;
            fadeImage.color = color;

            if (canvasGroup != null)
                canvasGroup.alpha = alpha;

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
    }
}

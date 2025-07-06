using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    [Header("Transition Object")]
    [SerializeField] private RectTransform transitionPanel;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float fullCoverDuration = 0.1f;

    [Header("Scene Event")]
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;

    private static SceneTransitionController _instance;

    private Vector2 aboveScreen;
    private Vector2 centerScreen = Vector2.zero;
    private Vector2 belowScreen;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializePositions();
    }

    private void InitializePositions()
    {
        float height = Screen.height;
        aboveScreen = new Vector2(0, height);
        belowScreen = new Vector2(0, -height);

        if (transitionPanel == null)
            return;

        transitionPanel.anchoredPosition = aboveScreen;
    }

    private void OnEnable()
    {
        if (sceneLoadEvent != null)
            sceneLoadEvent.OnRequestSceneLoad += HandleSceneLoad;
    }

    private void OnDisable()
    {
        if (sceneLoadEvent != null)
            sceneLoadEvent.OnRequestSceneLoad -= HandleSceneLoad;
    }

    private void HandleSceneLoad(SceneLoaderSO loader)
    {
        if (!string.IsNullOrEmpty(loader.sceneName))
            StartCoroutine(PerformSceneTransition(loader.sceneName));
    }

    private IEnumerator PerformSceneTransition(string sceneName)
    {
        Time.timeScale = 0f;

        yield return SlidePanel(aboveScreen, centerScreen);
        yield return new WaitForSecondsRealtime(fullCoverDuration);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f)
            yield return null;

        loadOp.allowSceneActivation = true;

        while (!loadOp.isDone)
            yield return null;

        Canvas.ForceUpdateCanvases();

        for (int i = 0; i < 3; i++)
            yield return new WaitForEndOfFrame();

        yield return SlidePanel(centerScreen, belowScreen);

        transitionPanel.anchoredPosition = aboveScreen;

        Time.timeScale = 1f;
    }

    private IEnumerator SlidePanel(Vector2 start, Vector2 end)
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            transitionPanel.anchoredPosition = Vector2.Lerp(start, end, elapsed / slideDuration);
            yield return null;
        }

        transitionPanel.anchoredPosition = end;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private RectTransform curtainWrapper;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float fullCoverDuration = 0.1f;
    [SerializeField] private float aspectRatioPadding = 0.2f;

    [Header("Scene Event")]
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;

    private static SceneTransitionController _instance;
    private Vector3 abovePos;
    private Vector3 centerPos = Vector3.zero;
    private Vector3 belowPos;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializePositions();
        curtainWrapper.localPosition = abovePos;
    }

    private void InitializePositions()
    {
        if (curtainWrapper == null) return;

        Canvas.ForceUpdateCanvases();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float idealHeight = screenWidth * (9f / 16f);
        float targetHeight = Mathf.Max(screenHeight, idealHeight) * (1f + aspectRatioPadding);

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null) targetHeight /= canvas.scaleFactor;

        abovePos = new Vector3(0, targetHeight, 0);
        belowPos = new Vector3(0, -targetHeight, 0);
    }

    private void OnEnable()
    {
        if (sceneLoadEvent != null) sceneLoadEvent.OnRequestSceneLoad += HandleSceneLoad;
    }

    private void OnDisable()
    {
        if (sceneLoadEvent != null) sceneLoadEvent.OnRequestSceneLoad -= HandleSceneLoad;
    }

    private void HandleSceneLoad(SceneLoaderSO loader)
    {
        if (isTransitioning) return;

        if (!string.IsNullOrEmpty(loader.sceneName))
        {
            StartCoroutine(PerformSceneTransition(loader.sceneName));
        }
    }

    private IEnumerator PerformSceneTransition(string sceneName)
    {
        isTransitioning = true;
        Time.timeScale = 0f;

        curtainWrapper.localPosition = abovePos;

        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        yield return SlideCurtain(abovePos, centerPos);
        yield return new WaitForSecondsRealtime(fullCoverDuration);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        loadOp.allowSceneActivation = false;

        while (loadOp.progress < 0.9f) yield return null;

        loadOp.allowSceneActivation = true;

        while (!loadOp.isDone) yield return null;

        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        yield return SlideCurtain(centerPos, belowPos);

        curtainWrapper.localPosition = abovePos;

        Time.timeScale = 1f;
        isTransitioning = false;
    }

    private IEnumerator SlideCurtain(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        curtainWrapper.localPosition = from;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            curtainWrapper.localPosition = Vector3.Lerp(from, to, elapsed / slideDuration);
            yield return null;
        }

        curtainWrapper.localPosition = to;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (curtainWrapper != null)
        {
            curtainWrapper.anchorMin = Vector2.zero;
            curtainWrapper.anchorMax = Vector2.one;
            curtainWrapper.pivot = new Vector2(0.5f, 0.5f);
            curtainWrapper.offsetMin = Vector2.zero;
            curtainWrapper.offsetMax = Vector2.zero;
        }
    }
#endif
}

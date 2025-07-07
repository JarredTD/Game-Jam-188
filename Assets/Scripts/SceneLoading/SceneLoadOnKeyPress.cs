using UnityEngine;

public class SceneLoadOnKeyPress : MonoBehaviour
{
    [Header("Scene Load Event")]
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;

    [Header("Reusable Loader Asset")]
    [SerializeField] private SceneLoaderSO loaderAsset;

    [Header("Key Configuration")]
    [SerializeField] private KeyCode triggerKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (sceneLoadEvent != null && loaderAsset != null)
            {
                loaderAsset.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

                Debug.Log($"'{triggerKey}' key pressed. Reloading scene: {loaderAsset.sceneName}");
                sceneLoadEvent.Raise(loaderAsset);
            }
            else
            {
                Debug.LogWarning("SceneLoadEvent or LoaderAsset is not assigned!");
            }
        }
    }
}

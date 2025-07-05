using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class SceneReloadTrigger : MonoBehaviour
{
    [Header("Scene Load Event")]
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;

    [Header("Reusable Loader Asset")]
    [SerializeField] private SceneLoaderSO loaderAsset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            loaderAsset.sceneName = currentScene;

            Debug.Log($"Player triggered reload of scene: {currentScene}");

            if (sceneLoadEvent != null && loaderAsset != null)
            {
                sceneLoadEvent.Raise(loaderAsset);
            }
            else
            {
                Debug.LogWarning("SceneReloadTrigger is missing sceneLoadEvent or loaderAsset reference.");
            }
        }
    }
}

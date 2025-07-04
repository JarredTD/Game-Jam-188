using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneLoadButton : MonoBehaviour
{
    [SerializeField] private SceneLoaderSO sceneToLoad;
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log($"Play button clicked, raising event for scene: {sceneToLoad?.sceneName}");
            if (sceneToLoad && sceneLoadEvent)
                sceneLoadEvent.Raise(sceneToLoad);
            else
                Debug.LogWarning("SceneToLoad or SceneLoadEvent is null!");
        });
    }

}

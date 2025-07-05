using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private SceneLoaderSO sceneToLoad;
    [SerializeField] private SceneLoadEventSO sceneLoadEvent;
    [SerializeField] private bool requirePlayerTag = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!sceneToLoad || !sceneLoadEvent)
        {
            Debug.LogWarning($"{name} is missing references!", this);
            return;
        }

        if (!requirePlayerTag || other.CompareTag("Player"))
        {
            Debug.Log($"{name} triggered by {other.name}, requesting scene load: {sceneToLoad.sceneName}");
            sceneLoadEvent.Raise(sceneToLoad);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scene/Scene Load Event")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<SceneLoaderSO> OnRequestSceneLoad;

    public void Raise(SceneLoaderSO loader)
    {
        OnRequestSceneLoad?.Invoke(loader);
    }
}

using UnityEngine;

public class ToggleUIListener : MonoBehaviour
{
    [SerializeField] private ToggleEventSO toggleEvent;
    [SerializeField] private bool hideOnEvent = false;
    [SerializeField] private GameObject targetWindow;

    private void OnEnable()
    {
        if (toggleEvent != null)
            toggleEvent.OnEventRaised += HandleToggle;
    }

    private void OnDisable()
    {
        if (toggleEvent != null)
            toggleEvent.OnEventRaised -= HandleToggle;
    }

    private void HandleToggle()
    {
        GameObject go = targetWindow != null ? targetWindow : gameObject;
        go.SetActive(!hideOnEvent);
    }
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RaiseToggleEventButton : MonoBehaviour
{
    [SerializeField] private ToggleEventSO toggleEvent;
    [SerializeField] private string debugLabel = "Unnamed UI Event";

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log($"[{debugLabel}] Button clicked. Raising toggle event.");

            if (toggleEvent != null)
            {
                toggleEvent.Raise();
            }
            else
            {
                Debug.LogWarning($"[{debugLabel}] ToggleEventSO is null on {gameObject.name}");
            }
        });
    }
}

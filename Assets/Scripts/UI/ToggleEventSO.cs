// ToggleEventSO.cs
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "UI/Toggle UI Event")]
public class ToggleEventSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void Raise()
    {
        OnEventRaised?.Invoke();
    }
}

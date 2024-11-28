using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TwoIntEventChannel", menuName = "EventChannel/TwoIntEventChannel")]
public class TwoIntEventChannel : ScriptableObject
{
    public event UnityAction<int, int> OnEventRaised;

    public void RaiseEvent(int value, int value2)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value, value2);
        }
    }
}

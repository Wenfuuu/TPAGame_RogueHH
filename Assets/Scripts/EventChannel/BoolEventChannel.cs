using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolEventChannel", menuName = "EventChannel/BoolEventChannel")]
public class BoolEventChannel : ScriptableObject
{
    public event UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
    }
}

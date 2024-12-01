using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DescEventChannel", menuName = "EventChannel/DescEventChannel")]
public class DescEventChannel : ScriptableObject
{
    public event UnityAction<bool, string, int> OnEventRaised;

    public void RaiseEvent(bool value, string desc, int lvl)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(value, desc, lvl);
        }
    }
}

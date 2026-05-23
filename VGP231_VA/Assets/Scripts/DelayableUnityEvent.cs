using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DelayableUnityEvent
{
    [Min(0.0f)]
    public float delay;

    public UnityEvent unityEvent;
}

[Serializable]
public class DelayableUnityEventArray
{
    public DelayableUnityEvent[] delaybleUnityEvents;
}
using System.Collections;
using UnityEngine;

public static class DelayableUnityEventUtility
{
    public static IEnumerator Invoke(DelayableUnityEvent delayedEvent)
    {
        if (delayedEvent.delay > 0.0f)
        {
            yield return new WaitForSeconds(delayedEvent.delay);
        }

        delayedEvent.unityEvent?.Invoke();
    }
}
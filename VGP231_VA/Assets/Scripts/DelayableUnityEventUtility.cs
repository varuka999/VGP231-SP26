using System.Collections;
using UnityEngine;

public static class DelayableUnityEventUtility
{
    public static void Invoke(MonoBehaviour runner, DelayableUnityEvent delayedEvent)
    {
        runner.StartCoroutine(InvokeCoroutine(delayedEvent));
    }

    public static IEnumerator InvokeCoroutine(DelayableUnityEvent delayedEvent)
    {
        if (delayedEvent.delay > 0f)
        {
            yield return new WaitForSeconds(delayedEvent.delay);
        }

        delayedEvent.unityEvent?.Invoke();
    }
}
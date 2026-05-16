using UnityEngine;
using UnityEngine.Events;

public class AwakeTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerOnAwake;

    private void Awake()
    {
        if (triggerOnAwake != null)
        {
            triggerOnAwake.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class TriggerVolume : MonoBehaviour
{
    public enum TriggerMode
    {
        Enter,
        Stay,
        Exit
    }

    [Header("Detection")]
    [SerializeField] private string targetTag = "Player";

    [Header("Mode")]
    [SerializeField] private TriggerMode triggerMode = TriggerMode.Enter;

    [Header("Event")]
    [SerializeField] private UnityEvent onTriggered;

    private bool playerInside;

    public bool PlayerInside => playerInside;

    private bool volumeConditionSatisfied;
    public bool VolumeConditionSatisfied => volumeConditionSatisfied;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        playerInside = true;

        if (triggerMode == TriggerMode.Enter)
        {
            onTriggered?.Invoke();
            volumeConditionSatisfied = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        if (triggerMode == TriggerMode.Stay)
        {
            onTriggered?.Invoke();
            volumeConditionSatisfied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        playerInside = false;

        if (triggerMode == TriggerMode.Exit)
        {
            onTriggered?.Invoke();
            volumeConditionSatisfied = true;
        }
        else if(triggerMode == TriggerMode.Stay)
        {
            volumeConditionSatisfied = false;
        }
    }
}
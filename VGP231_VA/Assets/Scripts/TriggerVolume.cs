using UnityEngine;
using UnityEngine.Events;

public class TriggerVolume : MonoBehaviour
{
    public enum TriggerMode
    {
        Enter,
        Stay,
        Exit,
        StayAndPressKey
    }

    [Header("Detection")]
    [SerializeField] private string targetTag = "Player";

    [Header("Mode")]
    [SerializeField] private TriggerMode triggerMode = TriggerMode.Enter;

    [Header("Interaction Key")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Event")]
    [SerializeField] private UnityEvent onTriggered;

    private bool playerInside;

    public bool PlayerInside => playerInside;

    private void Update()
    {
        if (triggerMode == TriggerMode.StayAndPressKey &&
            playerInside &&
            Input.GetKeyDown(interactKey))
        {
            onTriggered?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        playerInside = true;

        if (triggerMode == TriggerMode.Enter)
            onTriggered?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        if (triggerMode == TriggerMode.Stay)
            onTriggered?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        playerInside = false;

        if (triggerMode == TriggerMode.Exit)
            onTriggered?.Invoke();
    }
}
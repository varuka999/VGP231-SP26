using UnityEngine;
using UnityEngine.Events;
using static TriggerVolume;

public class InteractionSystem : MonoBehaviour
{
    private enum InteractionType
    {
        Once,
        Repeated
    }

    private TriggerVolume volumeScript;

    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private InteractionType interactionType = InteractionType.Once;

    [SerializeField] private GameObject interactionTextObject;

    [Header("Event")]
    [SerializeField] private UnityEvent onTriggered;

    private void Start()
    {
        volumeScript = GetComponent<TriggerVolume>();
    }

    private bool interactionComplete = false;
    public bool InteractionComplete => interactionComplete;

    private void Update()
    {
        if(!interactionComplete)
        {
            if (volumeScript.VolumeConditionSatisfied)
            {
                if(interactionTextObject != null)
                    interactionTextObject?.SetActive(true);

                if(Input.GetKeyDown(interactKey))
                {
                    onTriggered?.Invoke();

                    if (interactionType == InteractionType.Once)
                    {
                        interactionComplete = true;
                    }
                }
            }
            else if(!volumeScript.VolumeConditionSatisfied)
            {
                if (interactionTextObject != null)
                    interactionTextObject?.SetActive(false);
            }
        }
        else
        {
            if (interactionTextObject != null)
                interactionTextObject?.SetActive(false);
        }
    }
}

using UnityEngine;

public class ProgressionChecker : MonoBehaviour
{
    private enum CheckType
    {
        DialogueCheck,
        VolumeCheck
    }

    [SerializeField] private CheckType checkType = CheckType.DialogueCheck;

    private DialogueSystem dialogueScript;
    private TriggerVolume volumeScript;

    private void Start()
    {
        switch (checkType)
        {
            case CheckType.DialogueCheck:
                dialogueScript = GetComponent<DialogueSystem>();
                break;
            case CheckType.VolumeCheck:
                volumeScript = GetComponent<TriggerVolume>();
                break;
        }
    }

    private void Update()
    {
        switch (checkType)
        {
            case CheckType.DialogueCheck:
                if(dialogueScript != null && !dialogueScript.IsDialogueActive)
                {
                    ProgressScene();
                }
                break;
            case CheckType.VolumeCheck:
                if(dialogueScript != null && volumeScript.VolumeConditionSatisfied)
                {
                    ProgressScene();
                }
                break;
        }
    }

    private void ProgressScene()
    {
        ProgressionManager.Instance.IncrementProgression();
        this.enabled = false;
    }
}

using UnityEngine;

public class ProgressionChecker : MonoBehaviour
{
    private enum CheckType
    {
        DialogueCheck,
        None
    }

    [SerializeField] private CheckType checkType = CheckType.None;

    private DialogueSystem dialogueScript;

    private void Start()
    {
        switch (checkType)
        {
            case CheckType.DialogueCheck:
                dialogueScript = GetComponent<DialogueSystem>();
                break;
        }
    }

    private void Update()
    {
        switch (checkType)
        {
            case CheckType.None:
                break;
            case CheckType.DialogueCheck:
                if(dialogueScript != null && !dialogueScript.IsDialogueActive)
                {
                    ProgressScene();
                }
                break;
        }
    }

    public void ProgressScene()
    {
        ProgressionManager.Instance.IncrementProgression();
        this.enabled = false;
    }
}

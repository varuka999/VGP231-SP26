using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class AudioClipGroup
{
    public AudioClip[] clips;
}

public class DialogueSystem : MonoBehaviour
{
    [Header("Character TextMeshPro Elements")]
    [SerializeField] private TMP_Text[] textTargets;

    [Header("Dialogue Info")]
    [SerializeField] private string interactText = "...";
    [SerializeField] private AudioClipGroup[] characterTalkingClips;

    [System.Serializable]
    public class DialogueEntry
    {
        [TextArea(2, 5)]
        public string text;

        [Tooltip("Select which TMP_Text in textTargets to use")]
        public int targetIndex;

        public bool isCombatTrigger = false;
    }

    [System.Serializable]
    public class DialogueGroup
    {
        public DialogueEntry[] entries;
    }

    [Header("Dialogue Order")]
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private DialogueGroup[] randomDialoguePool;

    [SerializeField] private DelayableUnityEvent[] onDialogueEnd;

    [Header("Typing Settings")]
    [SerializeField] private float letterDelay = 0.03f;

    private int currentDialogueIndex = 0;
    private Coroutine typingRoutine;

    //private bool dialogueStarted;
    private bool isDialogueActive;
    public bool IsDialogueActive
    {
        get => isDialogueActive;

        private set
        {
            if (isDialogueActive == value)
                return;

            isDialogueActive = value;

            if (playerController != null)
            {
                playerController.SetMove(!isDialogueActive);
            }
        }
    }

    private bool hasDialogueEnded = false;
    public bool HasDialogueEnded => hasDialogueEnded;

    [Header("References")]
    private TriggerVolume volumeScript;
    private AudioSource talkingCharacterSource;
    private PlayerController playerController;
    private EnemyCombatHandler enemyCombatHandler = null;

    // Flags
    private bool textCleared = false;

    private void Start()
    {
        if (volumeScript == null)
        {
            volumeScript = GetComponent<TriggerVolume>();
        }

        playerController = FindAnyObjectByType<PlayerController>();
        enemyCombatHandler = GetComponent<EnemyCombatHandler>();
    }

    private void Update()
    {
        if (volumeScript != null && currentDialogueIndex == 0 && volumeScript.CurrentTriggerMode != TriggerVolume.TriggerMode.None)
        {
            if (volumeScript.PlayerInside)
            {
                textCleared = false;

                // assuming 0 for simplicity
                textTargets[0].text = interactText;
            }
            else if (!textCleared)
            {
                textCleared = true;
                ClearAllText();
            }
        }

        if (typingRoutine == null && talkingCharacterSource != null)
        {
            AudioManager.Instance.StopLoopingSound(talkingCharacterSource);
        }

        //playerController.SetMove(!IsDialogueActive);
    }

    public void PlayRandomDialogue(int randomDialoguePoolIndex)
    {
        // Disabling dialogue interruption for now for simplicity
        if (typingRoutine != null)
        {
            return;
        }

        if (randomDialoguePoolIndex < 0 || randomDialoguePoolIndex >= randomDialoguePool.Length)
        {
            Debug.LogWarning("Invalid random pool index on dialogue entry " + randomDialoguePoolIndex);
            return;
        }

        DialogueGroup group = randomDialoguePool[randomDialoguePoolIndex];

        if (group.entries == null || group.entries.Length == 0)
        {
            Debug.LogWarning("Dialogue group is empty.");
            return;
        }

        DialogueEntry entry = group.entries[Random.Range(0, group.entries.Length)];
        TMP_Text targetText = textTargets[entry.targetIndex];

        if (!IsTextClear(targetText))
        {
            EndDialogue();
            return;
        }

        ClearAllText();

        PlayDialogue(entry, false);
    }

    public void EndDialogue()
    {
        if (typingRoutine != null)
        {
            return;
        }

        ClearAllText();
        IsDialogueActive = false;
    }

    public void NextDialogue()
    {
        // Disabling dialogue interrupution for now for simplicity
        if (typingRoutine != null)
        {
            return;
        }

        ClearAllText();

        if (EndOfDialogueCheck())
        {
            return;
        }

        DialogueEntry entry = dialogueEntries[currentDialogueIndex];

        if (CombatTriggerCheck(entry))
        {
            return;
        }

        if (DebugManager.Instance.IsDebug)
        {
            while (!entry.isCombatTrigger && currentDialogueIndex < dialogueEntries.Length)
            {
                ++currentDialogueIndex;
                if (currentDialogueIndex < dialogueEntries.Length)
                {
                    entry = dialogueEntries[currentDialogueIndex];
                }
            }

            if (EndOfDialogueCheck())
            {
                return;
            }

            if (CombatTriggerCheck(entry))
            {
                return;
            }
        }

        if (entry.targetIndex < 0 || entry.targetIndex >= textTargets.Length)
        {
            Debug.LogWarning("Invalid target index on dialogue entry " + currentDialogueIndex);
            currentDialogueIndex++;
            return;
        }

        PlayDialogue(entry);
    }

    private bool EndOfDialogueCheck()
    {
        if (currentDialogueIndex >= dialogueEntries.Length)
        {
            Debug.Log("End of Dialogue!");
            IsDialogueActive = false;
            hasDialogueEnded = true;

            if (enemyCombatHandler != null)
            {
                enemyCombatHandler.ResetInteractionCollider();
            }

            for (int i = 0; i < onDialogueEnd.Length; ++i)
            {
                DelayableUnityEventUtility.Invoke(this, onDialogueEnd[i]);
            }

            return true;
        }

        return false;
    }

    private bool CombatTriggerCheck(DialogueEntry entry)
    {
        if (enemyCombatHandler != null)
        {
            if (enemyCombatHandler.InCombat)
            {
                return true;
            }
            else if (entry.isCombatTrigger)
            {
                IsDialogueActive = false;
                entry.isCombatTrigger = false;
                Debug.Log("Combat Start from Dialogue Trigger1!");
                enemyCombatHandler.CombatCycle();
                return true;
            }
        }
        return false;
    }

    private void PlayDialogue(DialogueEntry entry, bool incrementDialogueIndex = true)
    {
        IsDialogueActive = true;

        TMP_Text targetText = textTargets[entry.targetIndex];

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeTextReveal(targetText, entry.text));

        if (characterTalkingClips.Length > 0)
        {
            talkingCharacterSource = AudioManager.Instance.PlayLoopingSound(
            AudioManager.Instance.GetRandomSound(characterTalkingClips[entry.targetIndex].clips),
            textTargets[entry.targetIndex].transform.position);
        }

        if (!incrementDialogueIndex)
        {
            return;
        }

        currentDialogueIndex++;
    }

    IEnumerator TypeText(TMP_Text target, string message)
    {
        target.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            target.text += message[i];
            yield return new WaitForSeconds(letterDelay);
        }

        typingRoutine = null;
    }

    IEnumerator TypeTextReveal(TMP_Text target, string message)
    {
        // Set the complete text immediately
        target.text = message;

        // Force TMP to calculate the final layout
        target.ForceMeshUpdate();

        int totalCharacters = target.textInfo.characterCount;

        // Hide all characters
        target.maxVisibleCharacters = 0;

        // Reveal characters one by one
        for (int i = 0; i <= totalCharacters; i++)
        {
            target.maxVisibleCharacters = i;
            yield return new WaitForSeconds(letterDelay);
        }

        typingRoutine = null;
    }

    public void ResetDialogue()
    {
        currentDialogueIndex = 0;
    }

    public void ClearAllText()
    {
        foreach (TMP_Text t in textTargets)
        {
            if (t != null)
                t.text = "";
        }
    }

    public bool IsTextClear(TMP_Text target)
    {
        return target.text == "";
    }
}
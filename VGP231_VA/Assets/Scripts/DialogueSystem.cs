using System.Collections;
using TMPro;
using UnityEngine;

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

    [Header("Dialogue Order")]
    [SerializeField] private DialogueEntry[] dialogueEntries;

    [Header("Typing Settings")]
    [SerializeField] private float letterDelay = 0.03f;

    private int currentDialogueIndex = 0;
    private Coroutine typingRoutine;

    //private bool dialogueStarted;
    private bool isDialogueActive;
    public bool IsDialogueActive => isDialogueActive;

    [Header("References")]
    private TriggerVolume volumeScript;
    private AudioSource talkingCharacterSource;
    private PlayerController playerController;
    private EnemyCombatHandler enemyCombatHandler = null;

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
        if (volumeScript != null && currentDialogueIndex == 0)
        {
            if (volumeScript.PlayerInside)
            {
                // assuming 0 for simplicity
                textTargets[0].text = interactText;
            }
            else
            {
                ClearAllText();
            }
        }

        if (typingRoutine == null && talkingCharacterSource != null)
        {
            AudioManager.Instance.StopLoopingSound(talkingCharacterSource);
        }

        if (isDialogueActive)
        {
            playerController.SetMove(false);
        }
        else if (!isDialogueActive)
        {
            playerController.SetMove(true);
        }
    }

    public void NextDialogue()
    {
        // Disabling dialogue interrupution for now for simplicity
        if (typingRoutine != null)
        {
            return;
        }

        ClearAllText();


        if (currentDialogueIndex >= dialogueEntries.Length)
        {
            Debug.Log("End of Dialogue!");
            isDialogueActive = false;
            return;
        }

        DialogueEntry entry = dialogueEntries[currentDialogueIndex];

        if (enemyCombatHandler != null)
        {
            if (enemyCombatHandler.InCombat)
            {
                return;
            }
            else if (entry.isCombatTrigger)
            {
                isDialogueActive = false;
                entry.isCombatTrigger = false;
                Debug.Log("Combat Start from Dialogue Trigger!");
                enemyCombatHandler.CombatCycle();
                return;
            }
        }

        if (entry.targetIndex < 0 || entry.targetIndex >= textTargets.Length)
        {
            Debug.LogWarning("Invalid target index on dialogue entry " + currentDialogueIndex);
            currentDialogueIndex++;
            return;
        }

        isDialogueActive = true;
        TMP_Text targetText = textTargets[entry.targetIndex];

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeText(targetText, entry.text));

        talkingCharacterSource = AudioManager.Instance.PlayLoopingSound(
            AudioManager.Instance.GetRandomSound(characterTalkingClips[entry.targetIndex].clips),
            textTargets[entry.targetIndex].transform.position);

        currentDialogueIndex++;

        if (enemyCombatHandler != null && entry.isCombatTrigger)
        {
            Debug.Log("Combat Start from Dialogue Trigger!");
            enemyCombatHandler.CombatCycle();
            return;
        }
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
}
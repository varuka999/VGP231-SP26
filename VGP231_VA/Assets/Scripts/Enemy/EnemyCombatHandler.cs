using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatHandler : MonoBehaviour
{
    //combat cycle
    // dialogue limit
    [SerializeField] private List<GameObject> attackCycleGameObject = new List<GameObject>();
    [SerializeField] private List<int> dialogueCheckpoints = new List<int>();
    //[SerializeField] private GameObject enemyReference = null;
    //[SerializeField] private DialogueSystem dialogueSystem;
    //[SerializeField] private TriggerVolume triggerVolume;
    private int combatIndex = 0;
    private bool inCombat = false;

    public bool InCombat => inCombat;

    public void CombatCycle()
    {
        // start combat
        Debug.Log($"Combat Start!");
        inCombat = true;
        attackCycleGameObject[combatIndex].SetActive(true);
        this.gameObject.GetComponent<SphereCollider>().radius = 50.0f;
        ++combatIndex;
    }

    public void CombatCycleEnd()
    {
        inCombat = false;
        this.gameObject.GetComponent<DialogueSystem>().NextDialogue();
    }

    //public void CombatDialogue()
    //{
    //    for (int i = 0; i < dialogueCheckpoints[dialogueIndex]; ++i)
    //    {
    //        Debug.Log($"Combat Dialogue: {i} of {dialogueCheckpoints[dialogueIndex]}");
    //        dialogueSystem.NextDialogue();
    //    }

    //    CombatCycle();
    //}
}

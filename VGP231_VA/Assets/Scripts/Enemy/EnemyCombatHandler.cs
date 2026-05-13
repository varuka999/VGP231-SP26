using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatHandler : MonoBehaviour
{
    //combat cycle
    // dialogue limit
    [SerializeField] private List<GameObject> attackCycleGameObject = new List<GameObject>();
    private int combatIndex = 0;
    private bool inCombat = false;

    public bool InCombat => inCombat;

    public void CombatCycle()
    {
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
}

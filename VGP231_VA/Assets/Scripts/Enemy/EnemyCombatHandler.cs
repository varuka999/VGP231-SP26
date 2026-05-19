using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> attackCycleGameObject = new List<GameObject>();
    private int combatIndex = 0;
    private bool inCombat = false; // this is only refering to if the player is currently in an active attack cycle, not if the player is in combat with the enemy in general
    private bool isInEncounter = false; // this is refering to if the player is in combat with the enemy in general, not if the player is currently in an active attack cycle
    private float radiusStart = 0;
    public bool IsInEncounter => isInEncounter;
    private PlayerCombatHandler playerCombatHandler = null;

    private void Start()
    {
        playerCombatHandler = FindAnyObjectByType<PlayerCombatHandler>();
    }

    public bool InCombat => inCombat;

    public void CombatCycle()
    {
        Debug.Log($"Combat Start!");
        if (combatIndex == 0)
        {
            isInEncounter = true;
            playerCombatHandler.CombatStart();

            SphereCollider interactCollider = this.gameObject.GetComponent<SphereCollider>();
            if (interactCollider != null)
            {
                radiusStart = interactCollider.radius;
                interactCollider.radius = 50.0f;
            }
        }
         
        inCombat = true;
        attackCycleGameObject[combatIndex].SetActive(true);

        ++combatIndex;
    }

    public void CombatCycleEnd()
    {
        inCombat = false;
        if (combatIndex >= attackCycleGameObject.Count)
        {
            isInEncounter = false;
        }

        this.gameObject.GetComponent<DialogueSystem>().NextDialogue();
    }

    public void ResetInteractionCollider()
    {
        SphereCollider interactCollider = this.gameObject.GetComponent<SphereCollider>();
        if (interactCollider != null)
        {
            interactCollider.radius = radiusStart;
        }
    }
}

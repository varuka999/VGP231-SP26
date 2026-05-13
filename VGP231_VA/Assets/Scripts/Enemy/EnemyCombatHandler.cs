using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> attackCycleGameObject = new List<GameObject>();
    private int combatIndex = 0;
    private bool inCombat = false;
    private float radiusStart = 0;

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
            playerCombatHandler.CombatStart();
        }

        inCombat = true;
        attackCycleGameObject[combatIndex].SetActive(true);
        radiusStart = this.gameObject.GetComponent<SphereCollider>().radius;
        this.gameObject.GetComponent<SphereCollider>().radius = 50.0f;
        ++combatIndex;
    }

    public void CombatCycleEnd()
    {
        inCombat = false;
        this.gameObject.GetComponent<DialogueSystem>().NextDialogue();
    }

    public void ResetInteractionCollider()
    {
        this.gameObject.GetComponent<SphereCollider>().radius = radiusStart;
    }
}

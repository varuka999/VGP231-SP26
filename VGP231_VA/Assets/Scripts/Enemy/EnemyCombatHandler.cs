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

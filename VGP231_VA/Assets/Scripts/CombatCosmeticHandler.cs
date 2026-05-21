using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class CombatCosmeticHandler : MonoBehaviour
{
    private EnemyCombatHandler enemyCombatHandlerScript;

    private bool combatStarted;
    private bool combatEnded;

    [Header("Post Processing Setting")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private float combatTargetSaturation = 0.0f;
    [SerializeField] private float saturationDuration = 1.0f;

    [Header("Events")]
    [SerializeField] private UnityEvent onCombatStart;
    [SerializeField] private UnityEvent onCombatEnd;

    private float initialSaturation = 0.0f;

    private void Awake()
    {
        enemyCombatHandlerScript = GetComponent<EnemyCombatHandler>();
    }

    private void Update()
    {
        if (enemyCombatHandlerScript.InCombat && !combatStarted)
        {
            combatStarted = true;
            initialSaturation = PostProcessingManager.Instance.GetCurrentSaturation(postProcessVolume);
            PostProcessingManager.Instance.LerpToSaturation(postProcessVolume, combatTargetSaturation, saturationDuration);

            onCombatStart?.Invoke();
        }
        else if(combatStarted && !combatEnded && !enemyCombatHandlerScript.IsInEncounter)
        {
            combatEnded = true;
            PostProcessingManager.Instance.LerpToSaturation(postProcessVolume, initialSaturation, saturationDuration);

            onCombatEnd?.Invoke();
        }
    }
}

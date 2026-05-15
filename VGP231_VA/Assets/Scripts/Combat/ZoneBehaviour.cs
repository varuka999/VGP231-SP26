using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ZoneBehaviour : MonoBehaviour
{
    [SerializeField] private EnemyCombatHandler enemy = null;
    [SerializeField] private GameObject parent = null;
    [SerializeField] List<string> zonePatterns = new List<string>();
    [SerializeField] List<string> attackClips = new List<string>();
    [SerializeField] List<GameObject> attackDoTweens = new List<GameObject>();
    private int zoneCounter = 0;
    private int clipCounter = 0;
    private int doTweenCounter = 0;
    [SerializeField] Animator zoneAnimator;
    [SerializeField] Animator attackAnimator;

    void OnEnable()
    {
        PlayerNextZone();
    }

    public void PlayNextAttackClip()
    {
        attackAnimator.Play(attackClips[clipCounter]);
        ++clipCounter;
    }

    public void PlayNextAttackScript()
    {
        attackDoTweens[doTweenCounter].SetActive(true);
        ++doTweenCounter;
    }

    public void PlayerNextZone()
    {
        zoneAnimator.Play(zonePatterns[zoneCounter]);
        ++zoneCounter;
    }

    public void AttackCycleEnd()
    {
        enemy.CombatCycleEnd();
        parent.gameObject.SetActive(false);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ZoneBehaviour : MonoBehaviour
{
    // play next attack
    // if no more attacks, end of attack cycle

    [SerializeField] private EnemyCombatHandler enemy = null;
    [SerializeField] List<string> attackPatterns = new List<string>();
    [SerializeField] List<string> zonePatterns = new List<string>();
    [SerializeField] List<GameObject> doTweenAttacks = new List<GameObject>();
    public int count = 0;
    public int zCount = 0;
    public int dCount = 0;
    [SerializeField] Animator zoneAnimator;
    [SerializeField] Animator attackAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerNextZone();
    }

    public void PlayNextAttackClip()
    {
        attackAnimator.Play(attackPatterns[count]);
        ++count;
    }

    public void PlayNextAttackScript()
    {
        doTweenAttacks[dCount].SetActive(true);
        ++dCount;
    }

    public void PlayerNextZone()
    {
        zoneAnimator.Play(zonePatterns[zCount]);
        ++zCount;
    }

    public void AttackCycleEnd()
    {
        enemy.CombatCycleEnd();
    }
}

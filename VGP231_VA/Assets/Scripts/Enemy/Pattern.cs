using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    [SerializeField] List<string> attackPatterns = new List<string>();
    [SerializeField] List<string> zonePatterns = new List<string>();
    public int count = 0;
    public int zCount = 0;
    [SerializeField] Animator zoneAnimator;
    [SerializeField] Animator attackAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerNextZone();
    }

    public void PlayNextAttack()
    {
        attackAnimator.Play(attackPatterns[count]);
        ++count;
    }

    public void PlayerNextZone()
    {
        zoneAnimator.Play(zonePatterns[zCount]);
        ++zCount;
    }
}

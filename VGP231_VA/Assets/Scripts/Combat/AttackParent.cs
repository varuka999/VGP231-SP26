using DG.Tweening;
using UnityEngine;

public class AttackParent : MonoBehaviour
{
    public Transform end;
    public Ease easeType;
    public float duration;
    public MovementProfile movementType;

    [Header("Spawn Feel")]
    [SerializeField] public float anticipationDuration = 0.12f;
    [SerializeField] public float shakeStrength = 0.08f;
    [SerializeField] public int shakeVibrato = 12;

    [SerializeField] public GameObject damageZoneIndicator = null;
    [SerializeField] public float indicatorShowTime = 0.2f;

    [SerializeField] public float indicatorPulseScale = 1.08f;
    [SerializeField] public int indicatorFlashLoops = 2;
}
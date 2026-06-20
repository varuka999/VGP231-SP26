using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementProfile
{
    Straight,
    Final,
}

public class DoTweenAttack : MonoBehaviour
{
    [SerializeField] private AttackParent parent;
    private Transform end;
    private Ease easeType;
    private float duration;
    private MovementProfile movementType;

    private float anticipationDuration;
    private float shakeStrength;
    private int shakeVibrato;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private GameObject damageZoneIndicator;
    [SerializeField] private float indicatorPulseScale;
    [SerializeField] private int indicatorFlashLoops;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    private void OnEnable()
    {
        end = parent.end;
        easeType = parent.easeType;
        duration = parent.duration;
        movementType = parent.movementType;
        shakeStrength = parent.shakeStrength;
        shakeVibrato = parent.shakeVibrato;
        anticipationDuration = parent.anticipationDuration;
        damageZoneIndicator = parent.damageZoneIndicator;
        indicatorPulseScale = parent.indicatorPulseScale;
        indicatorFlashLoops = parent.indicatorFlashLoops;
        transform.position = originalPosition;

        StartAttack();
    }

    public void StartAttack()
    {
        switch (movementType)
        {
            case MovementProfile.Straight:
                Straight();
                break;
            case MovementProfile.Final:
                Final();
                break;
            default:
                break;
        }
    }

    public void Straight()
    {
        Sequence seq = DOTween.Sequence();

        Vector3 endPosition = end.position;

        transform.localScale = originalScale * 0.40f;

        if (damageZoneIndicator != null)
        {
            damageZoneIndicator.SetActive(true);
            damageZoneIndicator.transform.localScale = Vector3.one;
        }

        seq.Append(
            transform.DOScale(originalScale, anticipationDuration)
                .SetEase(Ease.OutBack)
        );

        seq.Join(
            transform.DOShakePosition(
                anticipationDuration,
                new Vector3(shakeStrength, 0f, shakeStrength),
                shakeVibrato,
                90f,
                false,
                true
            )
        );

        if (damageZoneIndicator != null)
        {
            seq.Join(
                damageZoneIndicator.transform
                    .DOScale(Vector3.one * indicatorPulseScale, anticipationDuration / (indicatorFlashLoops * 2.0f))
                    .SetLoops(indicatorFlashLoops * 2, LoopType.Yoyo)
                    .SetEase(Ease.OutQuad)
            );
        }

        float moveStartTime = seq.Duration();

        seq.AppendCallback(() =>
        {
            if (damageZoneIndicator != null)
            {
                damageZoneIndicator.SetActive(false);
            }
        });

        seq.Append(
            transform.DOMove(endPosition, duration)
                .SetEase(easeType)
        );

        float shrinkStartPercent = 0.70f;
        float shrinkStartTime = moveStartTime + duration * shrinkStartPercent;
        float shrinkTime = duration * (1.0f - shrinkStartPercent);

        seq.Insert(
            shrinkStartTime,
            transform.DOScale(Vector3.zero, shrinkTime)
                .SetEase(Ease.InBack)
        );

        seq.OnComplete(Delete);

        seq.Play();
    }

    public void Final()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOShakePosition(
                1.5f,                        // shake duration
                new Vector3(0.25f, 0f, 0f),  // shake strength/direction
                100,                         // vibrato: how many shakes
                90f,                         // randomness
                false,                       // snapping
                true                         // fade out
            )
        );

        seq.Append(
            transform.DOMove(
                new Vector3(end.position.x, end.position.y, end.position.z),
                duration
            )
            .SetEase(easeType)
        );
    }

    void Delete()
    {
        parent.gameObject.SetActive(false);
    }
}


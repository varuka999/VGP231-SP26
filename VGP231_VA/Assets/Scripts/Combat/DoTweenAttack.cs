using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementProfile
{
    Straight,
    VerticalArc,
    HorizontalArc,
}

public class DoTweenAttack : MonoBehaviour
{
    public Transform parent;
    public Transform end;
    public Ease easeType;
    public float duration;
    public MovementProfile movementType;

    private void OnEnable()
    {
        switch (movementType)
        {
            case MovementProfile.Straight:
                Straight();
                break;
            case MovementProfile.VerticalArc:
                break;
            case MovementProfile.HorizontalArc:
                HorizontalArc2();
                break;
            default:
                break;
        }
    }

    public void Loop()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(gameObject.transform.DOMoveY(-3.0f, 3.0f));
        seq.Append(gameObject.transform.DOMoveY(3.0f, 3.0f));
        seq.SetLoops(-1);

        seq.Play();
    }
    public void Move()
    {
        gameObject.transform.DOMoveY(-3.0f, 3.0f);
    }

    public void Straight()
    {
        Sequence seq = DOTween.Sequence();

        //seq.Append(gameObject.transform.DOLocalMoveY(-0.3f, 0.5f)).SetEase(Ease.InSine);
        //seq.Join(gameObject.transform.DOShakePosition(0.5f));
        seq.Append(gameObject.transform.DOMove(new Vector3(end.position.x, end.position.y, end.position.z), duration)).SetEase(easeType).OnComplete(Delete);

        seq.Play();
    }

    public void HorizontalArc()
    {
        Sequence seq1 = DOTween.Sequence();
        Sequence seq2 = DOTween.Sequence();
        //float startZ = gameObject.transform.position.z;
        float startZ = 0.5f;
        seq1.Append(gameObject.transform.DOMoveX(end.position.x, duration)).SetEase(easeType);
        seq2.Append(gameObject.transform.DOMoveZ(end.position.z + -2.0f, duration * 0.5f).SetEase(easeType)).OnComplete(() =>
        { gameObject.transform.DOMoveZ(startZ, duration * 0.5f).SetEase(easeType).OnComplete(Delete); });

        seq1.Play();
        seq2.Play();
    }

    public void HorizontalArc2()
    {
        float startZ = transform.position.z;
        float arcZ = startZ - 1.5f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveX(end.position.x, duration).SetEase(easeType));

        sequence.Join(transform.DOMoveZ(arcZ, duration * 0.5f).SetEase(Ease.OutQuad)
                .OnComplete(() => { transform.DOMoveZ(startZ, duration * 0.5f).SetEase(Ease.InQuad); }));

        sequence.OnComplete(Delete);
    }

    void Delete()
    {
        parent.gameObject.SetActive(false);
    }
}


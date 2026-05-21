using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweener : MonoBehaviour
{
    public enum TweenType
    {
        MoveTo,
        MoveOffset,
        RotateTo,
        RotateOffset,
        ScaleTo,
        ScaleOffset
    }

    [System.Serializable]
    public class TweenEntry
    {
        [Header("Target")]
        public Transform targetTransform;

        [Header("Tween Type")]
        public TweenType tweenType;

        [Header("Value")]
        public Vector3 value;

        [Header("Timing")]
        public float duration = 1f;
        public float delay = 0f;

        [Header("Ease")]
        public Ease easeType = Ease.Linear;
    }

    [Header("Tween Queue")]
    [SerializeField] private TweenEntry[] tweenEntries;
    private Queue<TweenEntry> tweenQueue = new Queue<TweenEntry>();

    [Header("Settings")]
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool destroyAtQueueEnd = true;
    [SerializeField] private bool useLocalTransform = true;

    private void Start()
    {
        BuildQueue();

        if (playOnStart)
        {
            StartCoroutine(PlayTweenQueue());
        }
    }

    private void BuildQueue()
    {
        tweenQueue.Clear();

        foreach (TweenEntry entry in tweenEntries)
        {
            tweenQueue.Enqueue(entry);
        }
    }

    public void PlayTweenQueueEvent()
    {
        BuildQueue();

        StartCoroutine(PlayTweenQueue());
    }

    public IEnumerator PlayTweenQueue()
    {
        while (tweenQueue.Count > 0)
        {
            TweenEntry currentTween = tweenQueue.Dequeue();

            Transform target = currentTween.targetTransform;

            if (target == null)
            {
                continue;
            }

            if (currentTween.delay > 0)
            {
                yield return new WaitForSeconds(currentTween.delay);
            }

            Tween tween = null;

            switch (currentTween.tweenType)
            {
                // MOVE TO
                case TweenType.MoveTo:

                    if (useLocalTransform)
                    {
                        tween = target.DOLocalMove(
                            currentTween.value,
                            currentTween.duration
                        );
                    }
                    else
                    {
                        tween = target.DOMove(
                            currentTween.value,
                            currentTween.duration
                        );
                    }
                    break;


                // MOVE OFFSET
                case TweenType.MoveOffset:

                    if (useLocalTransform)
                    {
                        tween = target.DOLocalMove(
                            target.localPosition + currentTween.value,
                            currentTween.duration
                        );
                    }
                    else
                    {
                        tween = target.DOMove(
                            target.position + currentTween.value,
                            currentTween.duration
                        );
                    }
                    break;

                // ROTATE TO
                case TweenType.RotateTo:

                    if (useLocalTransform)
                    {
                        tween = target.DOLocalRotate(
                            currentTween.value,
                            currentTween.duration
                        );
                    }
                    else
                    {
                        tween = target.DORotate(
                            currentTween.value,
                            currentTween.duration
                        );
                    }
                    break;

                // ROTATE OFFSET
                case TweenType.RotateOffset:

                    if (useLocalTransform)
                    {
                        tween = target.DOLocalRotate(
                            target.localEulerAngles + currentTween.value,
                            currentTween.duration
                        );
                    }
                    else
                    {
                        tween = target.DORotate(
                            target.eulerAngles + currentTween.value,
                            currentTween.duration
                        );
                    }
                    break;

                // SCALE TO
                case TweenType.ScaleTo:

                    tween = target.DOScale(
                        currentTween.value,
                        currentTween.duration
                    );
                    break;

                // SCALE OFFSET
                case TweenType.ScaleOffset:

                    tween = target.DOScale(
                        target.localScale + currentTween.value,
                        currentTween.duration
                    );

                    break;
            }

            if (tween != null)
            {
                tween.SetEase(currentTween.easeType);

                yield return tween.WaitForCompletion();

                if (destroyAtQueueEnd && tweenQueue.Count == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void AddTween(TweenEntry entry)
    {
        tweenQueue.Enqueue(entry);
    }

    public void ClearQueue()
    {
        tweenQueue.Clear();
    }

    public void RestartQueue()
    {
        StopAllCoroutines();

        BuildQueue();

        StartCoroutine(PlayTweenQueue());
    }
}
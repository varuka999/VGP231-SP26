using System.Collections;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string trigger;
    [SerializeField] private float triggerDelay = 0.0f;

    private bool animationTriggered = false;

    private TriggerVolume triggerVolume;

    private void Start()
    {
        triggerVolume = GetComponent<TriggerVolume>();
    }

    private void Update()
    {
        if(triggerVolume != null && triggerVolume.VolumeConditionSatisfied && !animationTriggered)
        {
            StartCoroutine(TriggerAnimation(trigger));
        }
    }

    public void TriggerAnimationEvent()
    {
        StartCoroutine(TriggerAnimation(trigger));
    }

    public void TriggerAnimationEvent(string triggerName)
    {
        StartCoroutine(TriggerAnimation(triggerName));
    }

    IEnumerator TriggerAnimation(string triggerName)
    {
        yield return new WaitForSeconds(triggerDelay);

        animationTriggered = true;
        animator.SetTrigger(triggerName);
    }
}

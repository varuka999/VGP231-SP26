using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string trigger;

    private bool animationTriggered = false;

    private TriggerVolume triggerVolume;

    private void Start()
    {
        triggerVolume = GetComponent<TriggerVolume>();
    }

    private void Update()
    {
        if(triggerVolume.VolumeConditionSatisfied && !animationTriggered)
        {
            animationTriggered = true;
            animator.SetTrigger(trigger);
        }
    }
}

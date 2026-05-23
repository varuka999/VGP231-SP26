using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [System.Serializable]
    private enum SwitchType
    {
        Once,
        OnlyInVolume
    }

    [SerializeField] private CinemachineCamera fromCamera;
    [SerializeField] private CinemachineCamera toCamera;

    [SerializeField] private SwitchType switchType = SwitchType.Once;

    private TriggerVolume triggerVolume;

    private void Start()
    {
        TryGetComponent(out triggerVolume);
    }

    private void Update()
    {
        if(triggerVolume != null)
        {
            SwitchCameras();
        }
    }

    public void SwitchCameras()
    {
        if (triggerVolume.VolumeConditionSatisfied)
        {
            SwitchForward();
        }
        else if (!triggerVolume.VolumeConditionSatisfied && switchType == SwitchType.OnlyInVolume)
        {
            SwitchBackward();
        }
    }

    public void SwitchForward()
    {
        CameraManager.Instance.SwitchCameras(fromCamera, toCamera);
    }

    public void SwitchBackward()
    {
        CameraManager.Instance.SwitchCameras(toCamera, fromCamera);
    }
}

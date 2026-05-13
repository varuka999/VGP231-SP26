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
        triggerVolume = GetComponent<TriggerVolume>();
    }

    private void Update()
    {
        if(triggerVolume.VolumeConditionSatisfied)
        {
            CameraManager.Instance.SwitchCameras(fromCamera, toCamera);
        }
        else if(!triggerVolume.VolumeConditionSatisfied && switchType == SwitchType.OnlyInVolume)
        {
            CameraManager.Instance.SwitchCameras(toCamera, fromCamera);
        }
    }
}

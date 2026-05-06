using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Transition Settings")]
    public float duration = 0.4f;
    public float zoomedFOV = 30f;
    public float tiltAmount = -20f;

    [HideInInspector] public bool cutThisFrame = false;

    bool busy = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        // resets automatically after one frame
        cutThisFrame = false;
    }

    public void SwitchCameras(CinemachineCamera from, CinemachineCamera to, int activePriority = 20, int inactivePriority = 0)
    {
        if (from != null)   
            from.Priority = inactivePriority;

        if (to != null)
            to.Priority = activePriority;
    }

    public void SwitchToCamera(CinemachineCamera to, int activePriority = 20, int inactivePriority = 0)
    {

    }

    public void SetCameraPriority(CinemachineCamera cam, int priority)
    {
        if (cam == null)
        {
            Debug.LogWarning("SetCameraPriority: Camera is null.");
            return;
        }

        cam.Priority = priority;
    }
    public void DoTransition(CinemachineCamera camA, CinemachineCamera camB)
    {
        if (busy) return;
        StartCoroutine(Transition(camA, camB));
    }

    IEnumerator Transition(CinemachineCamera camA, CinemachineCamera camB)
    {
        busy = true;

        float camAFov = camA.Lens.FieldOfView;
        float camBFov = camB.Lens.FieldOfView;

        Quaternion camARot = camA.transform.localRotation;
        Quaternion camBRot = camB.transform.localRotation;

        float t = 0f;

        // CAMERA A
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / duration);

            camA.Lens.FieldOfView =
                Mathf.Lerp(camAFov, zoomedFOV, lerp);

            camA.transform.localRotation =
                Quaternion.Slerp(
                    camARot,
                    camARot * Quaternion.Euler(tiltAmount, 0f, 0f),
                    lerp
                );

            yield return null;
        }

        // CUT FRAME
        camA.Priority = 0;
        camB.Priority = 100;

        cutThisFrame = true;

        camB.Lens.FieldOfView = zoomedFOV;
        camB.transform.localRotation =
            camBRot * Quaternion.Euler(tiltAmount, 0f, 0f);

        yield return null;

        // CAMERA B
        t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / duration);

            camB.Lens.FieldOfView =
                Mathf.Lerp(zoomedFOV, camBFov, lerp);

            camB.transform.localRotation =
                Quaternion.Slerp(
                    camBRot * Quaternion.Euler(tiltAmount, 0f, 0f),
                    camBRot,
                    lerp
                );

            yield return null;
        }

        camA.Lens.FieldOfView = camAFov;
        camA.transform.localRotation = camARot;

        camB.Lens.FieldOfView = camBFov;
        camB.transform.localRotation = camBRot;

        busy = false;
    }
}
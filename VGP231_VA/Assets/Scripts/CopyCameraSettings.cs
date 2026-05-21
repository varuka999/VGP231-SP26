using UnityEngine;

[ExecuteAlways]
public class CopyCameraSettings : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Camera overlayCamera;

    private void LateUpdate()
    {
        if (targetCamera == null || overlayCamera == null)
        {
            return;
        }

        overlayCamera.fieldOfView = targetCamera.fieldOfView;

        overlayCamera.nearClipPlane = targetCamera.nearClipPlane;

        overlayCamera.farClipPlane = targetCamera.farClipPlane;

        overlayCamera.orthographic = targetCamera.orthographic;

        overlayCamera.orthographicSize = targetCamera.orthographicSize;
    }
}
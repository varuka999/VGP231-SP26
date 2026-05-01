using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [Header("Target Camera")]
    private Camera targetCamera;

    [Header("Axis Lock")]
    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null)
        {
            return;
        }

        Vector3 direction = transform.position - targetCamera.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 euler = lookRotation.eulerAngles;

        if (lockX) { euler.x = transform.eulerAngles.x; }
        if (lockY) { euler.y = transform.eulerAngles.y; }
        if (lockZ) { euler.z = transform.eulerAngles.z; }

        transform.rotation = Quaternion.Euler(euler);
    }
}
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    private Camera mainCamera;

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite playerSpriteFront;
    [SerializeField] private Sprite playerSpriteBack;

    [Header("Rotation")]
    private float targetYRotation;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Axis Lock")]
    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;
    [SerializeField] private bool lockZ = false;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
        HandleSpriteSwitching();
        HandleFlipping();
    }

    void HandleSpriteSwitching()
    {
        if (spriteRenderer == null) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            spriteRenderer.sprite = playerSpriteBack;
            targetYRotation = 180f;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            spriteRenderer.sprite = playerSpriteFront;
            targetYRotation = 0f;
        }
    }

    void HandleFlipping()
    {
        if (playerController.Velocity.sqrMagnitude > 0.01f)
        {
            float dot = Vector3.Dot(mainCamera.transform.right, playerController.Velocity);

            if (Mathf.Abs(dot) > 0.01f)
            {
                targetYRotation = dot > 0 ? 0f : 180f;
            }
        }

        // Billboard toward camera
        Quaternion targetRot =
            Quaternion.LookRotation(mainCamera.transform.forward)
            * Quaternion.Euler(0f, targetYRotation, 0f);

        // Smooth rotation
        Quaternion smoothRot = Quaternion.Lerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotationSpeed
        );

        // Apply axis locks
        Vector3 euler = smoothRot.eulerAngles;

        if (lockX) euler.x = transform.eulerAngles.x;
        if (lockY) euler.y = transform.eulerAngles.y;
        if (lockZ) euler.z = transform.eulerAngles.z;

        transform.rotation = Quaternion.Euler(euler);
    }

    public void LockBillboardingAxes(int axis)
    {
        switch(axis)
        {
            case 1:
                lockX = !lockX;
                break;
            case 2:
                lockY = !lockY;
                break;
            case 3:
                lockZ = !lockZ;
                break;
            default:
                break;
        }
    }
}
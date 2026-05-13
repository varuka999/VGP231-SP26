using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    private Camera mainCamera;

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite playerSpriteFront;
    [SerializeField] private Sprite playerSpriteBack;

    private float targetYRotation;
    [SerializeField] private float rotationSpeed = 10f;

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
            targetYRotation = 180f; // flip when going "back"
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            spriteRenderer.sprite = playerSpriteFront;
            targetYRotation = 0f; // normal facing
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

        // Smoothly rotate toward target
        Quaternion targetRot = Quaternion.LookRotation(mainCamera.transform.forward)
                             * Quaternion.Euler(0f, targetYRotation, 0f);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }
}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    [Header("References")]
    public Transform sprite;
    public Camera cam;

    private Vector3 velocity;
    private Vector2 moveInput;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        HandleMovement();
        HandleFacing();
    }

    void HandleMovement()
    {
        // Get camera directions
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        // Flatten them so we don't move vertically
        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Convert input into camera-relative direction
        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;

        Vector3 targetVelocity = moveDir * moveSpeed;

        if (moveInput.magnitude > 0.01f)
        {
            velocity = Vector3.Lerp(velocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        transform.position += velocity * Time.deltaTime;
    }

    void HandleFacing()
    {
        // Flip based on horizontal movement only (Paper Mario style)
        if (sprite != null && velocity.sqrMagnitude > 0.01f)
        {
            float dot = Vector3.Dot(cam.transform.right, velocity);

            if (Mathf.Abs(dot) > 0.01f)
            {
                Vector3 scale = sprite.localScale;
                scale.x = Mathf.Sign(dot) * Mathf.Abs(scale.x);
                sprite.localScale = scale;
            }
        }
    }
}
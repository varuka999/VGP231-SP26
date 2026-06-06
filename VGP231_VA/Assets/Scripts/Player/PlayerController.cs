using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    private bool disableMove = false;
    [SerializeField] private bool enableJitter = false;

    [Header("References")]
    public Camera cam;
    [SerializeField] private Rigidbody rb = null;

    private Vector3 velocity = Vector3.zero;
    public Vector3 Velocity => velocity;

    private Vector2 moveInput = Vector2.zero;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (rb == null)
        {
            return;
        }

        if (disableMove)
        {
            velocity = Vector3.MoveTowards(
                velocity,
                Vector3.zero,
                deceleration * Time.fixedDeltaTime
            );

            rb.linearVelocity = new Vector3(0.0f, rb.linearVelocity.y, 0.0f);
            return;
        }

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        // Flatten them so we don't move vertically
        camForward.y = 0.0f;
        camRight.y = 0.0f;

        camForward.Normalize();
        camRight.Normalize();

        // Convert input into camera-relative direction
        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;

        if (moveDir.sqrMagnitude > 1.0f)
        {
            moveDir.Normalize();
        }

        Vector3 targetVelocity = moveDir * moveSpeed;

        float rate = moveInput.magnitude > 0.01f ? acceleration : deceleration;

        velocity = Vector3.MoveTowards(
            velocity,
            targetVelocity,
            rate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    public void SetMove(bool move)
    {
        disableMove = !move;

        if (disableMove)
        {
            velocity = Vector3.zero;

            if (rb != null && !enableJitter)
            {
               //rb.linearVelocity = new Vector3(0.0f, rb.linearVelocity.y, 0.0f);
            }
        }
    }
}
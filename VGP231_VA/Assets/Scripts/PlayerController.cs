using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    private bool disableMove = false;

    [Header("References")]
    public Camera cam;

    private Vector3 velocity;
    public Vector3 Velocity => velocity;
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

        if(!disableMove)
        {
            HandleMovement();
        }
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

    public void SetMove(bool move)
    {
        disableMove = !move;
    }
}
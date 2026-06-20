using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float acceleration = 10f;
    public float deceleration = 15f;

    [Header("Collision")]
    [SerializeField] private float playerRadius = 0.25f;
    [SerializeField] private float skinWidth = 0.03f;
    [SerializeField] private LayerMask collisionMask;

    private bool disableMove = false;

    [Header("References")]
    public Camera cam;

    private Vector3 velocity;
    public Vector3 Velocity => velocity;

    private Vector2 moveInput;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (!disableMove)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;

        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir.Normalize();
        }

        Vector3 targetVelocity = moveDir * moveSpeed;

        if (moveInput.magnitude > 0.01f)
        {
            velocity = Vector3.Lerp(
                velocity,
                targetVelocity,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            velocity = Vector3.Lerp(
                velocity,
                Vector3.zero,
                deceleration * Time.deltaTime
            );
        }

        Vector3 movement = velocity * Time.deltaTime;
        MoveWithCollision(movement);
    }

    private void MoveWithCollision(Vector3 movement)
    {
        if (movement.sqrMagnitude <= 0.000001f)
        {
            return;
        }

        Vector3 moveDirection = movement.normalized;
        float moveDistance = movement.magnitude;

        if (Physics.SphereCast(
            transform.position,
            playerRadius,
            moveDirection,
            out RaycastHit hit,
            moveDistance + skinWidth,
            collisionMask
        ))
        {
            float allowedDistance = Mathf.Max(hit.distance - skinWidth, 0f);

            transform.position += moveDirection * allowedDistance;

            Vector3 remainingMovement = movement - moveDirection * allowedDistance;

            Vector3 slideMovement = Vector3.ProjectOnPlane(remainingMovement,hit.normal);

            if (slideMovement.sqrMagnitude > 0.000001f)
            {
                Vector3 slideDirection = slideMovement.normalized;
                float slideDistance = slideMovement.magnitude;

                if (Physics.SphereCast(
                    transform.position,
                    playerRadius,
                    slideDirection,
                    out RaycastHit slideHit,
                    slideDistance + skinWidth,
                    collisionMask
                ))
                {
                    float allowedSlideDistance = Mathf.Max(slideHit.distance - skinWidth, 0f);
                    transform.position += slideDirection * allowedSlideDistance;
                }
                else
                {
                    transform.position += slideMovement;
                }
            }

            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        }
        else
        {
            transform.position += movement;
        }
    }

    public void SetMove(bool move)
    {
        disableMove = !move;

        if (disableMove)
        {
            velocity = Vector3.zero;
        }
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
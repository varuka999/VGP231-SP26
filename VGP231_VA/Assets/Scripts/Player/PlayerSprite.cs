using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerSprite : MonoBehaviour
{
    private Camera mainCamera;

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite playerSpriteFront;
    [SerializeField] private Sprite playerSpriteBack;

    [Header("Attack Settings")]
    [SerializeField] private Material normalSpriteMat;
    [SerializeField] private Material hitSpriteMat;

    //[SerializeField] private bool startFacingFront;

    [Header("Rotation")]
    private float targetYRotation;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Axis Lock")]
    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;
    [SerializeField] private bool lockZ = false;

    [Header("Turn Events")]
    [SerializeField] private UnityEvent frontTurnEvents;
    [SerializeField] private UnityEvent backTurnEvents;

    [SerializeField] private bool enableTurnEvents = false;

    private bool isFacingFront = false;
    public bool IsFacingFront => isFacingFront;

    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = transform.parent.GetComponent<PlayerController>();

        //if(startFacingFront)
        //{
        //    FaceFront();
        //}
        //else
        //{
        //    FaceBack();
        //}       
    }

    void LateUpdate()
    {
        HandleSpriteSwitching();
        HandleFlipping();
    }

    void HandleSpriteSwitching()
    {
        if (spriteRenderer == null) return;

        if (isFacingFront && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            FaceBack();
        }
        else if (!isFacingFront && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            FaceFront();
        }
    }

    public void FaceBack()
    {
        spriteRenderer.sprite = playerSpriteBack;
        targetYRotation = 180f;

        isFacingFront = false;

        if(enableTurnEvents)
        {
            backTurnEvents?.Invoke();
        }
    }

    public void FaceFront()
    {
        spriteRenderer.sprite = playerSpriteFront;
        targetYRotation = 0f;

        isFacingFront = true;

        if(enableTurnEvents)
        {
            frontTurnEvents?.Invoke();
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

    public void SetFrontFacingSprite(Sprite frontSprite)
    {
        playerSpriteFront = frontSprite;
    }

    public void SetBackFacingSprite(Sprite backSprite)
    {
        playerSpriteBack = backSprite;
    }

    public void PlayerHitEvent()
    {
        StartCoroutine(PlayerHit());
    }

    IEnumerator PlayerHit()
    {
        spriteRenderer.material = hitSpriteMat;

        transform.DOKill();

        Sequence hitSeq = DOTween.Sequence();

        hitSeq.Append(
            transform.DOScale(
                new Vector3(1.25f, 0.75f, 1f),
                0.06f
            )
        );

        hitSeq.Join(
            transform.DOLocalMoveX(0.25f, 0.05f)
                .SetRelative()
        );

        hitSeq.Append(
            transform.DOScale(
                Vector3.one,
                0.25f
            ).SetEase(Ease.OutElastic)
        );

        hitSeq.Join(
            transform.DOLocalMoveX(-0.25f, 0.35f)
                .SetRelative()
                .SetEase(Ease.OutElastic)
        );

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.material = normalSpriteMat;
    }

    public void SetTurnEvents(bool turnEvents)
    {
        enableTurnEvents = turnEvents;
    }
}
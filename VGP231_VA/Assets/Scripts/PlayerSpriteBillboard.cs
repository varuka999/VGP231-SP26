using UnityEngine;

public class PlayerSpriteBillboard : MonoBehaviour
{
    private Camera mainCamera;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite playerSpriteFront;
    [SerializeField] private Sprite playerSpriteBack;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if(spriteRenderer != null)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                spriteRenderer.sprite = playerSpriteBack;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                spriteRenderer.sprite = playerSpriteFront;
            }
        }

        if (mainCamera == null)
        {
            return;
        }

        transform.forward = mainCamera.transform.forward;
    }
}
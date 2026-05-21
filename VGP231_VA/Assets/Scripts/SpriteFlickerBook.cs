using System.Collections;
using UnityEngine;

public class SpriteFlickerBook : MonoBehaviour
{
    [Header("Flicker Settings")]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float flickerInterval = 0.05f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnStart = true;

    private SpriteRenderer spriteRenderer;
    private Sprite initialSprite;
    private Coroutine flickerRoutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
    }

    private void Start()
    {
        if (playOnStart)
            StartFlicker();
    }

    public void StartFlicker()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(Flicker());
    }

    public void RemoveFlicker()
    {
        StopFlicker();
        spriteRenderer.sprite = initialSprite;
    }
    
    public void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }
    }

    private IEnumerator Flicker()
    {
        if (sprites == null || sprites.Length == 0)
            yield break;

        do
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                spriteRenderer.sprite = sprites[i];
                yield return new WaitForSeconds(flickerInterval);
            }
        }
        while (loop);

        flickerRoutine = null;
    }
}
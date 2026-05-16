using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombatHandler : MonoBehaviour
{
    [Header("Health")]
    private int healthCounter = 0;
    public int HealthCounter => healthCounter;
    public string sceneName = string.Empty;

    [Header("Damage VFX")]
    public SpriteRenderer playerSprite = null;
    public float damageFlashDuration = 0.15f;

    private Color originalColor = Color.white;
    private Tween damageFlashTween = null;

    private void Awake()
    {
        if (playerSprite != null)
        {
            originalColor = playerSprite.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            other.gameObject.GetComponent<AttackCollision>().DeactivateCollision();
            TakeDamage();
        }
    }

    public void CombatStart()
    {
        healthCounter = 3;
    }

    private void TakeDamage()
    {
        --healthCounter;

        PlayDamageFlash();

        // play sfx & audio

        if (healthCounter <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayDamageFlash()
    {
        if (playerSprite == null)
        {
            Debug.Log("Sprite Not Found");
            return;
        }

        damageFlashTween?.Kill();

        playerSprite.color = Color.red;

        damageFlashTween = playerSprite.DOColor(originalColor, damageFlashDuration).SetEase(Ease.OutQuad);
    }

    private void PlayerDeath()
    {
        // play sfx & audio
        // prompt retry
        // temp
        SceneManager.LoadScene(sceneName);
    }
}
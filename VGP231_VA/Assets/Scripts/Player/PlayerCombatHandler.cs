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
    public GameObject damageIndicator = null;
    public Transform end;

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
        GameObject damageInd = Instantiate(
            damageIndicator,
            new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z),
            Quaternion.identity,
            this.gameObject.transform
        );

        damageFlashTween = damageInd.transform
            .DOMove(new Vector3(end.position.x, end.position.y, end.position.z), 0.6f)
            .SetEase(Ease.InQuad)
            .OnComplete(() => Delete(damageInd));

        damageFlashTween.Play();
    }

    void Delete(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void PlayerDeath()
    {
        // play sfx & audio
        // prompt retry
        // temp
        SceneManager.LoadScene(sceneName);
    }
}
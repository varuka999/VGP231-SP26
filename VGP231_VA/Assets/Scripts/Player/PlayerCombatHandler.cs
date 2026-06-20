using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombatHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [Header("Health")]
    private int healthCounter = 9999;
    public int HealthCounter => healthCounter;
    public string sceneName = string.Empty;

    [Header("Damage VFX")]
    public SpriteRenderer playerSprite = null;
    public float damageFlashDuration = 0.15f;

    private Color originalColor = Color.white;
    private Tween damageFlashTween = null;
    public GameObject damageIndicator = null;
    public Transform end;

    [SerializeField] private DelayableUnityEventArray[] hitEventsDelayArray;
    [SerializeField] private DelayableUnityEventArray[] dieEventsDelayArray;


    private void Awake()
    {
        if (playerSprite != null)
        {
            originalColor = playerSprite.color;
        }
        if (playerController == null)
        {
            playerController = this.gameObject.GetComponent<PlayerController>();
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
        if(playerController != null)
        {
            playerController.SetMoveSpeed(1.8f);
        }

        healthCounter = 5; //--------------------------------------------
    }

    public void CombatEnd()
    {
        if (playerController != null)
        {
            playerController.SetMoveSpeed(1.5f);
        }
    }

    private void TakeDamage()
    {
        --healthCounter;

        PlayDamageFlash();

        // play sfx & audio
        RunDelayableUnityEventArray(hitEventsDelayArray);
        CameraManager.Instance.ShakeActiveCamera();

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
        RunDelayableUnityEventArray(dieEventsDelayArray);

        TransitionManager.Instance.TransitionToSceneEvent(sceneName);

        //SceneManager.LoadScene(sceneName);
    }

    public void RunDelayableUnityEventArray(DelayableUnityEventArray[] eventsDelayArray)
    {
        for (int i = 0; i < eventsDelayArray.Length; ++i)
        {
            for (int j = 0; j < eventsDelayArray[i].delaybleUnityEvents.Length; ++j)
            {
                DelayableUnityEventUtility.Invoke(this, eventsDelayArray[i].delaybleUnityEvents[j]);
            }
        }
    }
}
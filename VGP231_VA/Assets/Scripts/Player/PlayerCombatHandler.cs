using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombatHandler : MonoBehaviour
{
    private int healthCounter = 0;
    public int HealthCounter => healthCounter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
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
        // play sfx & audio
        // check whether dead
        if (healthCounter <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        // play sfx & audio
        // prompt retry
        // temp
        SceneManager.LoadScene("Scenario-Test");
    }
}
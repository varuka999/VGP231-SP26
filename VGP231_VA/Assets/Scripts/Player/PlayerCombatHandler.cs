using UnityEngine;

public class PlayerCombatHandler : MonoBehaviour
{
    private int healthCounter = 0;
    public int HealthCounter => healthCounter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Attack")
        {

        }
    }

    public void CombatStart()
    {
        healthCounter = 3;
    }

    private void UpdateHealth()
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

    }
}

using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private EnemyCombatHandler enemy = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (enemy != null)
            {
                enemy.ReduceObjectiveCounter();
                this.gameObject.SetActive(false);
            }
        }
    }
}

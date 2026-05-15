using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public void DeactivateCollision()
    {
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }
}

using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    public void DeactivateCOllision()
    {
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }
}

using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private GameObject currentRoom;
    [SerializeField] private GameObject targetRoom;

    [SerializeField] private Transform targetPlayerPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            currentRoom.SetActive(false);
            targetRoom.SetActive(true);

            other.transform.position = targetPlayerPos.position;
        }
    }
}

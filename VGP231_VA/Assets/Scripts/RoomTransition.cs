using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    [SerializeField] private GameObject currentRoom;
    [SerializeField] private GameObject targetRoom;

    [SerializeField] private CinemachineCamera currentCam;
    [SerializeField] private CinemachineCamera targetCam;

    [SerializeField] private Transform targetPlayerPos;

    [SerializeField] private AudioClip[] transitionSoundClips;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeScene(other.transform));
        }
    }

    private IEnumerator ChangeScene(Transform playerTransform)
    {
        CameraManager.Instance.DoTransition(currentCam, targetCam);
        AudioManager.Instance.PlaySoundInSpace(AudioManager.Instance.GetRandomSound(transitionSoundClips), gameObject.transform.position);

        yield return new WaitUntil(() => CameraManager.Instance.cutThisFrame);

        currentRoom.SetActive(false);
        targetRoom.SetActive(true);

        playerTransform.position = targetPlayerPos.position;
    }
}

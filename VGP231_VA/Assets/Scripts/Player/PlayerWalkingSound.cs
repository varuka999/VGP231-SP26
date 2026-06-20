using UnityEngine;
using UnityEngine.Audio;

public class PlayerWalkingSound : MonoBehaviour
{
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private float volume = 1f;
    [SerializeField] private AudioMixerGroup mixerGroup;

    public bool isWalking = false;

    private AudioSource walkingAudioSource;

    private void Update()
    {
        if (isWalking)
        {
            if (walkingAudioSource == null)
            {
                walkingAudioSource = AudioManager.Instance.PlayLoopingSound(
                    walkingClip,
                    transform.position,
                    volume,
                    mixerGroup);
            }
            else
            {
                // Keep the sound attached to the player.
                walkingAudioSource.transform.position = transform.position;
            }
        }
        else
        {
            if (walkingAudioSource != null)
            {
                AudioManager.Instance.StopLoopingSound(walkingAudioSource);
                walkingAudioSource = null;
            }
        }
    }

    public void SetWalking(bool walking)
    {
        isWalking = walking;
    }
}
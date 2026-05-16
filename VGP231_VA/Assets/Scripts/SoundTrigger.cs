using System.Collections;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] clipsToPlay;
    [SerializeField] private Transform[] clipPositions;
    [SerializeField] private float[] clipDelays = { 0.0f };
    [SerializeField] private float clipVolume = 1.0f;

    [SerializeField] private bool playOnce = true;
    // if false, then only plays one randomly
    [SerializeField] private bool playAllAtOnce = false;

    private int soundPlayedCount = 0;

    public void PlaySound()
    {
        if (playOnce && soundPlayedCount > 0)
        {
            this.enabled = false;
            return;
        }

        ++soundPlayedCount;

        if(!playAllAtOnce)
        {
            StartCoroutine(PlaySoundWithDelay(AudioManager.Instance.GetRandomSound(clipsToPlay), clipPositions[0], clipVolume, clipDelays[0]));
            return;
        }

        for (int i = 0; i < clipsToPlay.Length; i++)
        {
            StartCoroutine(PlaySoundWithDelay(clipsToPlay[i], clipPositions[i], clipVolume, clipDelays[i]));
        }
    }

    public IEnumerator PlaySoundWithDelay(AudioClip clipToPlay, Transform source, float clipVolume, float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioManager.Instance.PlaySoundInSpace(clipToPlay, source.position, clipVolume);
    }
}

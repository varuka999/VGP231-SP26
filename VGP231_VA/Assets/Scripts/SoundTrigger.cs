using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] clipsToPlay;
    [SerializeField] private Transform[] clipPositions;
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
            AudioManager.Instance.PlaySoundInSpace(AudioManager.Instance.GetRandomSound(clipsToPlay), clipPositions[0].position, clipVolume);
            return;
        }

        for (int i = 0; i < clipsToPlay.Length; i++)
        {
            AudioManager.Instance.PlaySoundInSpace(clipsToPlay[i], clipPositions[i].position, clipVolume);
        }
    }
}

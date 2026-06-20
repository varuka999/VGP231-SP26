using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField][Range(0f, 1f)] private float volume = 1f;

    public void PlayMusic()
    {
        MusicManager.Instance.PlayMusic(musicClip, volume);
    }
}
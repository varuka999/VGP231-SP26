using UnityEngine;


[DefaultExecutionOrder(-15)]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlaySoundInSpace(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }

    public AudioSource PlayLoopingSound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
        {
            return null;
        }

        GameObject soundObject = new GameObject(clip.name + "Looping Sound");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f;
        audioSource.Play();

        // Keep reference to stop it later
        return audioSource;
    }

    public void StopLoopingSound(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
            Destroy(source.gameObject);
        }
    }

    public AudioClip GetRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("GetRandomSound called with null or empty clips array.");
            return null;
        }

        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }
}
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-15)]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioMixerGroup sfxGroup;

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

    public void PlaySoundInSpace(
    AudioClip clip,
    Vector3 position,
    float volume = 1f,
    AudioMixerGroup overrideGroup = null)
    {
        if (clip == null) return;

        GameObject soundObject = new GameObject(clip.name + " OneShot");
        soundObject.transform.position = position;

        AudioSource source = soundObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f;

        source.outputAudioMixerGroup = overrideGroup != null
            ? overrideGroup
            : sfxGroup;

        source.Play();

        Destroy(soundObject, clip.length);
    }

    public AudioSource PlayLoopingSound(
    AudioClip clip,
    Vector3 position,
    float volume = 1f,
    AudioMixerGroup overrideGroup = null)
    {
        if (clip == null) return null;

        GameObject soundObject = new GameObject(clip.name + " Looping Sound");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f;

        audioSource.outputAudioMixerGroup = overrideGroup != null
            ? overrideGroup
            : sfxGroup;

        audioSource.Play();

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
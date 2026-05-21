using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public struct Music
    {
        public AudioClip audioClip;
        public float volume;
    }

    [System.Serializable]
    public struct Ambience
    {
        public Rooms room;
        public Music ambience;
    }

    private static MusicManager _instance;
    public static MusicManager Instance { get { return _instance; } }

    [SerializeField]
    private Ambience[] roomsAmbience = new Ambience[System.Enum.GetValues(typeof(Rooms)).Length];

    [SerializeField] private float crossfadeDuration = 2.0f;

    private AudioSource _audioSourceA;
    private AudioSource _audioSourceB;

    private AudioSource _activeSource;
    private Coroutine _crossfadeCoroutine;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        _audioSourceA = CreateAudioSource("Music Source A");
        _audioSourceB = CreateAudioSource("Music Source B");

        _activeSource = _audioSourceA;
    }

    private AudioSource CreateAudioSource(string sourceName)
    {
        GameObject sourceObject = new GameObject(sourceName);
        sourceObject.transform.SetParent(transform);

        AudioSource source = sourceObject.AddComponent<AudioSource>();

        source.loop = true;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.volume = 0f;

        return source;
    }

    public void PlayRoomAmbience(int roomIndex)
    {
        for (int i = 0; i < roomsAmbience.Length; i++)
        {
            if ((int)roomsAmbience[i].room == roomIndex)
            {
                Music roomAmbience = roomsAmbience[i].ambience;

                PlayMusic(roomAmbience.audioClip, roomAmbience.volume);

                return;
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public void PlayMusicEvent(AudioClip clip)
    {
        PlayMusic(clip, 1.0f);
    }

    public void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null)
        {
            return;
        }

        if (_activeSource.clip == clip && _activeSource.isPlaying)
        {
            return;
        }

        AudioSource newSource = _activeSource == _audioSourceA ? _audioSourceB : _audioSourceA;

        newSource.clip = clip;
        newSource.volume = 0f;
        newSource.Play();

        if (_crossfadeCoroutine != null)
        {
            StopCoroutine(_crossfadeCoroutine);
        }

        _crossfadeCoroutine =
            StartCoroutine(Crossfade(_activeSource, newSource, volume));

        _activeSource = newSource;
    }

    private IEnumerator Crossfade(AudioSource oldSource, AudioSource newSource, float targetVolume)
    {
        float timer = 0f;

        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / crossfadeDuration;

            if (oldSource != null)
            {
                oldSource.volume = Mathf.Lerp(targetVolume, 0f, t);
            }

            if (newSource != null)
            {
                newSource.volume = Mathf.Lerp(0f, targetVolume, t);
            }

            yield return null;
        }

        if (oldSource != null)
        {
            oldSource.Stop();
            oldSource.clip = null;
            oldSource.volume = 0f;
        }

        if (newSource != null)
        {
            newSource.volume = targetVolume;
        }
    }

    public void StopMusic()
    {
        if (_crossfadeCoroutine != null)
        {
            StopCoroutine(_crossfadeCoroutine);
        }

        _audioSourceA.Stop();
        _audioSourceB.Stop();

        _audioSourceA.clip = null;
        _audioSourceB.clip = null;
    }

    public void FadeOutAndStopMusic()
    {
        FadeOutAndStopMusic(crossfadeDuration);
    }

    public void FadeOutAndStopMusic(float fadeDuration)
    {
        if (_crossfadeCoroutine != null)
        {
            StopCoroutine(_crossfadeCoroutine);
        }

        _crossfadeCoroutine = StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        AudioSource sourceA = _audioSourceA;
        AudioSource sourceB = _audioSourceB;

        float startVolumeA = sourceA.volume;
        float startVolumeB = sourceB.volume;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            sourceA.volume = Mathf.Lerp(startVolumeA, 0f, t);
            sourceB.volume = Mathf.Lerp(startVolumeB, 0f, t);

            yield return null;
        }

        sourceA.Stop();
        sourceB.Stop();

        sourceA.clip = null;
        sourceB.clip = null;

        sourceA.volume = 0f;
        sourceB.volume = 0f;
    }

    public void PauseMusic()
    {
        _audioSourceA.Pause();
        _audioSourceB.Pause();
    }

    public void ResumeMusic()
    {
        _audioSourceA.UnPause();
        _audioSourceB.UnPause();
    }

    public bool IsPlaying()
    {
        return _audioSourceA.isPlaying || _audioSourceB.isPlaying;
    }

    public AudioClip GetRandomMusic(Music[] music)
    {
        return AudioManager.Instance.GetRandomSound(MusicArrayToClips(music));
    }

    private AudioClip[] MusicArrayToClips(Music[] music)
    {
        if (music == null || music.Length == 0)
        {
            return null;
        }

        AudioClip[] clips = new AudioClip[music.Length];

        for (int i = 0; i < music.Length; i++)
        {
            clips[i] = music[i].audioClip;
        }

        return clips;
    }
}
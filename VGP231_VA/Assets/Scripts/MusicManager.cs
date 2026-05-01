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

    private AudioSource _audioSource;

    [SerializeField] private Ambience[] roomsAmbience = new Ambience[System.Enum.GetValues(typeof(Rooms)).Length];

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

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.spatialBlend = 0f; // 2D music
        _audioSource.playOnAwake = false;
    }

    public void PlayRoomAmbience(int roomIndex)
    {
        for (int i = 0; i < roomsAmbience.Length; i++)
        {
            if ((int)roomsAmbience[i].room == roomIndex)
            {
                StopMusic();

                Music roomAmbience = roomsAmbience[i].ambience;
                PlayMusic(roomAmbience.audioClip, roomAmbience.volume);
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if ((_audioSource.clip == clip && _audioSource.isPlaying) || clip == null)
        {
            return;
        }

        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
        _audioSource.clip = null;
    }

    public void PauseMusic()
    {
        _audioSource.Pause();
    }

    public void ResumeMusic()
    {
        if (_audioSource.clip != null)
        {
            _audioSource.UnPause();
        }
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = Mathf.Clamp01(volume);
    }

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
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
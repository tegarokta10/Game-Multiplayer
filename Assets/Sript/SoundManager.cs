using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource bgMusicSource;
    public AudioSource sfxSource; // Audio source khusus untuk SFX
    public AudioClip lobbyMusic;
    public AudioClip inGameMusic;
    public AudioClip shootSFX; // AudioClip untuk efek suara tembakan

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLobbyMusic()
    {
        PlayMusic(lobbyMusic);
    }

    public void PlayInGameMusic()
    {
        PlayMusic(inGameMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (bgMusicSource.clip != clip)
        {
            bgMusicSource.clip = clip;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }
    }

    public void StopMusic()
    {
        bgMusicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip); // Memainkan efek suara sekali
    }
}

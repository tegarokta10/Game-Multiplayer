using UnityEngine;

public class InGameSound : MonoBehaviour
{
    public static InGameSound Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgMusicSource; // AudioSource untuk musik latar
    public AudioSource sfxSource;     // AudioSource untuk efek suara

    [Header("Audio Clips")]
    public AudioClip inGameMusic;     // Musik latar in-game
    public AudioClip shootSFX;        // Efek suara untuk tembakan

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Menjaga InGameSound tetap ada di seluruh scene
        }
        else
        {
            Destroy(gameObject); // Menghindari duplikasi
        }
    }

    private void Start()
    {
        // Mulai memainkan musik latar in-game saat permainan dimulai
        PlayInGameMusic();
    }

    // Fungsi untuk memainkan musik latar in-game
    public void PlayInGameMusic()
    {
        if (bgMusicSource != null && inGameMusic != null)
        {
            bgMusicSource.clip = inGameMusic;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }
    }

    // Fungsi untuk menghentikan musik latar
    public void StopInGameMusic()
    {
        if (bgMusicSource != null)
        {
            bgMusicSource.Stop();
        }
    }

    // Fungsi untuk memainkan efek suara tembakan
    public void PlayShootSFX()
    {
        if (sfxSource != null && shootSFX != null)
        {
            sfxSource.PlayOneShot(shootSFX);
        }
    }
}

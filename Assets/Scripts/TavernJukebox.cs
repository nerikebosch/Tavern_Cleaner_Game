using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TavernJukebox : MonoBehaviour
{
    // This creates a "Singleton" so we never accidentally have two overlapping music players
    public static TavernJukebox instance;

    [Header("Your Music Tracks")]
    public AudioClip[] playlist;

    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Awake()
    {
        // 1. The "DontDestroyOnLoad" Trick!
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive when switching scenes!
        }
        else
        {
            // If we load the Main Menu again, delete the duplicate music player
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Start the first song
        if (playlist.Length > 0)
        {
            PlayNextTrack();
        }
    }

    void Update()
    {
        // 2. Check if the song has finished playing
        if (!audioSource.isPlaying && playlist.Length > 0)
        {
            currentTrackIndex++; // Move to the next song

            // If we reached the end of the playlist, loop back to song 0
            if (currentTrackIndex >= playlist.Length)
            {
                currentTrackIndex = 0;
            }

            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        audioSource.clip = playlist[currentTrackIndex];
        audioSource.Play();
    }
}
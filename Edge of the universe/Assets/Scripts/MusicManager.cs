using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public float fadeDuration = 2f;
    public float musicVolume = 0.1f;
    public AudioClip first;

    private AudioSource activeSource;
    private AudioSource fadeSource;

    void Start()
    {
        Debug.Log("MusicManager is active.");
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create two AudioSources for crossfading
        activeSource = gameObject.AddComponent<AudioSource>();
        fadeSource = gameObject.AddComponent<AudioSource>();
        fadeSource.Stop();
        fadeSource.clip = null;
        fadeSource.volume = 0f;


        activeSource.loop = true;
        fadeSource.loop = true;


        Debug.Log($"ActiveSource playing: {activeSource.isPlaying}, Clip: {activeSource.clip?.name}");
        Debug.Log($"FadeSource playing: {fadeSource.isPlaying}, Clip: {fadeSource.clip?.name}");

    }

    public void PlayMusic(AudioClip newClip)
    {
        Debug.Log($"Trying to play clip: {newClip?.name}");

        if (newClip == null || activeSource.clip == newClip)
        {
            Debug.Log($"Couldnt play audio clid");

            return;
        }

        StopAllCoroutines();
        StartCoroutine(CrossfadeMusic(newClip));
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        // Swap sources
        AudioSource oldSource = activeSource;
        AudioSource newSource = fadeSource;

        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.Play();

        float time = 0f;
        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            oldSource.volume = Mathf.Lerp(musicVolume, 0f, t);
            newSource.volume = Mathf.Lerp(0f, musicVolume, t);
            time += Time.deltaTime;
            yield return null;
        }

        oldSource.Stop();
        oldSource.volume = 0f;

        // Swap roles
        activeSource = newSource;
        fadeSource = oldSource;
    }
}

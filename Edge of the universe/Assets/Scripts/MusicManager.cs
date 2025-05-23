using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public float fadeDuration = 2f;

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

        activeSource.loop = true;
        fadeSource.loop = true;
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
            oldSource.volume = Mathf.Lerp(1f, 0f, t);
            newSource.volume = Mathf.Lerp(0f, 1f, t);
            time += Time.deltaTime;
            yield return null;
        }

        oldSource.Stop();
        oldSource.volume = 1f;

        // Swap roles
        activeSource = newSource;
        fadeSource = oldSource;
    }
}

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Réglages Filtre")]
    public float normalFreq = 22000f;
    public float muffledFreq = 800f;
    public float transitionSpeed = 6f;

    private AudioLowPassFilter lowPassFilter;
    private float targetFrequency;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        lowPassFilter = GetComponent<AudioLowPassFilter>();
        targetFrequency = normalFreq;
        lowPassFilter.cutoffFrequency = normalFreq;
    }

    void Update()
    {

        if (Mathf.Abs(lowPassFilter.cutoffFrequency - targetFrequency) > 10f)
        {
            lowPassFilter.cutoffFrequency = Mathf.Lerp(
                lowPassFilter.cutoffFrequency,
                targetFrequency,
                Time.deltaTime * transitionSpeed
            );
        }
    }

    public void MuffleMusic(bool shouldMuffle)
    {
        if (shouldMuffle)
            targetFrequency = muffledFreq;
        else
            targetFrequency = normalFreq;
    }
}
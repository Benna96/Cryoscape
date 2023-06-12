using UnityEngine;
using UnityEngine.Audio;

public class StartFade : MonoBehaviour
{
    public AudioMixer masterMixer; // Reference to the AudioMixer
    public AudioMixerSnapshot startSnapshot; // Snapshot at the start of the scene
    public AudioMixerSnapshot endSnapshot; // Snapshot at the end of the fade

    public float fadeDuration = 1.0f; // Duration of the fade in seconds

    private void Start()
    {
        // Transition to the start snapshot immediately
        startSnapshot.TransitionTo(0f);

        // Start the fade coroutine
        StartCoroutine(FadeAndSlide());
    }

    private System.Collections.IEnumerator FadeAndSlide()
    {
        yield return new WaitForSeconds(fadeDuration);

        // Transition to the end snapshot over the fade duration
        endSnapshot.TransitionTo(fadeDuration);
    }
}

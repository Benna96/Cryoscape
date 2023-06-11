using UnityEngine;

public class AudioFader : MonoBehaviour
{
    public string playerTag; // Tag of the GameObject with the player
    public AudioSource audioSource; // AudioSource to fade in/out
    public float fadeDuration = 1.0f; // Duration of the fade in/out in seconds

    private bool isFading; // Flag to track if fading is in progress
    private float fadeTimer; // Timer for the fade effect
    private float targetVolume; // Target volume for fading

    private void Start()
    {
        audioSource.volume = 0f; // Start with volume at 0
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Player entered the trigger, initiate fade-in
            isFading = true;
            fadeTimer = 0f;
            targetVolume = 1f;
            audioSource.Play();
           
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Player exited the trigger, initiate fade-out
            isFading = true;
            fadeTimer = 0f;
            targetVolume = 0f;
           
        }
    }

    private void Update()
    {
        if (isFading)
        {
            Fade();
        }
    }

    private void Fade()
    {
        if (fadeTimer <= fadeDuration)
        {
            fadeTimer += Time.deltaTime;

            // Calculate the fade factor based on the elapsed time
            float fadeFactor = fadeTimer / fadeDuration;

            // Smoothly adjust the volume towards the target volume
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, fadeFactor);
        }
        else
        {
            // Fade-in/out is complete
            audioSource.volume = targetVolume;
            isFading = false;

            if (targetVolume == 0f)
            {
                audioSource.Stop();
                
            }
            else
            {
                
            }
        }
    }
}

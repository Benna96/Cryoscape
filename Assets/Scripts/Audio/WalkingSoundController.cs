using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingSoundController : MonoBehaviour
{
    public AudioSource walkingAudioSource;
    public AudioClip walkingSound;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        // Initialize the audio source and any other necessary setup
    }

    private void Update()
    {
        Vector2 movementInput = playerMovement.movementInput;

        if (movementInput != Vector2.zero)
        {
            PlayWalkingSound();
        }
        else
        {
            StopWalkingSound();
        }
    }

    public void PlayWalkingSound()
    {
        if (!walkingAudioSource.isPlaying)
        {
            walkingAudioSource.clip = walkingSound;
            walkingAudioSource.Play();
        }
    }

    public void StopWalkingSound()
    {
        walkingAudioSource.Stop();
    }
}

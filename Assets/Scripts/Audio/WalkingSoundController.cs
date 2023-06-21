using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingSoundController : MonoBehaviour
{
    public AudioSource walkingAudioSource;
    public AudioSource runningAudioSource;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        // Initialize any other necessary setup
    }

    private void Update()
    {
        Vector2 movementInput = playerMovement.movementInput;

        if (movementInput != Vector2.zero)
        {
            PlayFootstepSound();
        }
        else
        {
            StopFootstepSound();
        }
    }

    public void PlayFootstepSound()
    {
        if (playerMovement.sprinting)
        {
            if (!runningAudioSource.isPlaying)
            {
                runningAudioSource.Play();
            }
        }
        else
        {
            if (!walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Play();
            }
        }
    }

    public void StopFootstepSound()
    {
        walkingAudioSource.Stop();
        runningAudioSource.Stop();
    }
}

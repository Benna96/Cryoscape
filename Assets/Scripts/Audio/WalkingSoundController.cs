using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingSoundController : MonoBehaviour
{
    public AudioClip walkingSound;
    public AudioClip runningSound;

    public GameObject walkingAudioSourceObject;
    public GameObject runningAudioSourceObject;

    private PlayerMovement playerMovement;
    private AudioSource walkingAudioSource;
    private AudioSource runningAudioSource;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        walkingAudioSource = walkingAudioSourceObject.GetComponent<AudioSource>();
        runningAudioSource = runningAudioSourceObject.GetComponent<AudioSource>();
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
            if (!runningAudioSource.isPlaying || runningAudioSource.clip != runningSound)
            {
                runningAudioSource.clip = runningSound;
                runningAudioSource.Play();
            }
        }
        else
        {
            if (!walkingAudioSource.isPlaying || walkingAudioSource.clip != walkingSound)
            {
                walkingAudioSource.clip = walkingSound;
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

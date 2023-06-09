using UnityEngine;
using UnityEngine.Audio;

public class MusicFadeTrigger : MonoBehaviour
{
    public AudioMixer mixer; // Reference to the Audio Mixer
    public string fadeInSnapshotName; // Name of the snapshot for fading in the music
    public string fadeOutSnapshotName; // Name of the snapshot for fading out the music

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Fade in the desired music track
            mixer.FindSnapshot(fadeInSnapshotName).TransitionTo(1.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Fade out the current music track
            mixer.FindSnapshot(fadeOutSnapshotName).TransitionTo(1.0f);
        }
    }
}

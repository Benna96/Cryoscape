using UnityEngine;

public class ToxicWave : MonoBehaviour
{
    public float waveSpeed = 1f;        // Speed of the wave movement
    public float waveIntensity = 1f;    // Intensity of the wave effect
    public float forwardSpeed = 1f;     // Speed of the forward and backward movement
    public float forwardDistance = 1f;  // Distance of the forward and backward movement

    private float initialY;             // Initial Y position of the sprite
    private float initialX;             // Initial X position of the sprite

    void Start()
    {
        initialY = transform.position.y;    // Store the initial Y position of the sprite
        initialX = transform.position.x;    // Store the initial X position of the sprite
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave function
        float newY = initialY + Mathf.Sin(Time.time * waveSpeed) * waveIntensity;

        // Calculate the new X position using a sine wave function
        float newX = initialX + Mathf.Sin(Time.time * forwardSpeed) * forwardDistance;

        // Update the sprite's position
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}

using UnityEngine;

public class CentrifugeRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float wobbleAmount = 0.08f;
    public float wobbleSpeed = 0.2f;

    private float originalY;

    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        // Rotate the object around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Apply wobbling effect
        float wobbleOffset = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        Vector3 newPosition = transform.position;
        newPosition.y = originalY + wobbleOffset;
        transform.position = newPosition;
    }
}

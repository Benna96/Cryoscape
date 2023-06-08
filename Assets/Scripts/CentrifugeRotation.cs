using UnityEngine;

public class CentrifugeRotation : MonoBehaviour
{
    public float rotationSpeed = 100.0f;

    private void Update()
    {
        // Rotate the object around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

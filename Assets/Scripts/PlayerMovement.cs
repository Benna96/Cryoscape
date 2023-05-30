using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float movementSpeed = 6f;

    private Vector2 movementInput;

    private void FixedUpdate()
    {
        rigidBody.velocity = movementSpeed * ((transform.forward * movementInput.y) + (transform.right * movementInput.x));
    }

    /// <summary>
    /// Called by Player Input component when movement direciton changes
    /// </summary>
    public void UpdateMovementDirection(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}

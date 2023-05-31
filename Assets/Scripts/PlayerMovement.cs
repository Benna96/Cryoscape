using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float movementSpeed = 6f;

    private Vector2 movementInput;

    private void FixedUpdate()
    {
        rigidBody.velocity = movementSpeed * ((transform.forward * movementInput.y) + (transform.right * movementInput.x));
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnCrouch()
    {
        Debug.Log("Pressed crouch button");
    }
}

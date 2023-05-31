using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float crouchSpeed = 3f;
    float startScaleY;
    float crouchScaleY = 0.5f;
    bool crouched;

    private Vector2 movementInput;

    private void Start() 
    {
        startScaleY = transform.localScale.y;
        crouched = false;
    }
    private void FixedUpdate()
    {
        if (!crouched)
        {
            rigidBody.velocity = movementSpeed * ((transform.forward * movementInput.y) + (transform.right * movementInput.x));
        } else {
            rigidBody.velocity = crouchSpeed * ((transform.forward * movementInput.y) + (transform.right * movementInput.x));
        }
        
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnCrouch()
    {
        if (!crouched)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchScaleY, transform.localScale.z);
            rigidBody.AddForce(Vector3.down * 20f, ForceMode.Impulse);
            crouched = true;
        } else {
            transform.localScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
            crouched = false;
        }
        Debug.Log("Pressed crouch button");
    }
}

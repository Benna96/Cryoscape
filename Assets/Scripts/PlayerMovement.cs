using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float crouchSpeed = 3f;
    float startScaleY;
    float crouchScaleY = 0.5f;
    bool crouched;

    private Vector2 movementInput;

    private void Start() 
    {
        startScaleY = modelTransform.localScale.y;
        crouched = false;
    }
    private void FixedUpdate()
    {
        if (!crouched)
        {
            rigidBody.velocity = movementSpeed * ((modelTransform.forward * movementInput.y) + (modelTransform.right * movementInput.x));
        } else {
            rigidBody.velocity = crouchSpeed * ((modelTransform.forward * movementInput.y) + (modelTransform.right * movementInput.x));
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
            modelTransform.localScale = new Vector3(modelTransform.localScale.x, crouchScaleY, modelTransform.localScale.z);
            rigidBody.AddForce(Vector3.down * 20f, ForceMode.Impulse);
            crouched = true;
        } else {
            modelTransform.localScale = new Vector3(modelTransform.localScale.x, startScaleY, modelTransform.localScale.z);
            crouched = false;
        }
        Debug.Log("Pressed crouch button");
    }
}

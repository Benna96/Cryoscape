using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform actualModel;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float movementSpeed = 6f;
    private float crouchSpeed;
    float startScaleY;
    float crouchScaleY;
    public bool crouched { get; private set; }
    public bool sprinting { get; private set; }

    public Vector2 movementInput;

    private void Start()
    {
        startScaleY = modelTransform.localScale.y;
        crouchScaleY = startScaleY / 2;
        crouchSpeed = movementSpeed / 2;
        crouched = false;
        sprinting = false;
    }

    private void FixedUpdate()
    {
        Vector3 wantedVelocity;

        if (crouched)
        {
            wantedVelocity = crouchSpeed * ((modelTransform.forward * movementInput.y) +
                                    (modelTransform.right * movementInput.x));
        }
        else if (sprinting)
        {
            wantedVelocity = (1.5f * movementSpeed) * ((modelTransform.forward * movementInput.y) +
                                    (modelTransform.right * movementInput.x));
        }
        else
        {
            wantedVelocity = movementSpeed * ((modelTransform.forward * movementInput.y) +
                                    (modelTransform.right * movementInput.x));
        }

        rigidBody.velocity = new(wantedVelocity.x, rigidBody.velocity.y, wantedVelocity.z); // Causes jumping to happen if colliders don't line up perfectly...
        rigidBody.velocity = wantedVelocity; // So nvm, let's limit y too afterall
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    /// <param name="context"></param>
    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    public void Crouch()
    {
        if (!crouched)
        {
            modelTransform.localScale = new Vector3(modelTransform.localScale.x, crouchScaleY, modelTransform.localScale.z);
            rigidBody.AddForce(Vector3.down * 20f, ForceMode.Impulse);
            crouched = true;
        }
        else
        {
            sprinting = false; // No sprinting when coming out of crouch

            var castOrigin = actualModel.position; //+ new Vector3(0, crouchScaleY, 0);
            float rayLength = 8f;
            if (Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, rayLength))
            {
                var distanceToCeiling = hit.point.y - castOrigin.y;
                if (hit.distance > rayLength / 2)
                {
                    modelTransform.localScale = new Vector3(modelTransform.localScale.x, startScaleY, modelTransform.localScale.z);
                    crouched = false;
                }
            }
            else
            {
                modelTransform.localScale = new Vector3(modelTransform.localScale.x, startScaleY, modelTransform.localScale.z);
                crouched = false;
            }
        }
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            sprinting = !sprinting;

            WalkingSoundController soundController = GetComponent<WalkingSoundController>();
            if (sprinting)
            {
                soundController.StopFootstepSound(); // Stop any existing footstep sound
                soundController.PlayFootstepSound();
            }
            else
            {
                soundController.StopFootstepSound(); // Stop any existing footstep sound
                soundController.PlayFootstepSound();
            }
        }
    }
}

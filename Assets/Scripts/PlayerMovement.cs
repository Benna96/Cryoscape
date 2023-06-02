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
    bool crouched;

    private Vector2 movementInput;

    private void Start() 
    {
        startScaleY = modelTransform.localScale.y;
        crouchScaleY = startScaleY / 2;
        crouchSpeed = movementSpeed / 2;
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
            } else {
                modelTransform.localScale = new Vector3(modelTransform.localScale.x, startScaleY, modelTransform.localScale.z);
                crouched = false;
            }
        }
        Debug.Log("Pressed crouch button");
    }
    void Update() 
    {
        Vector3 Upwards = modelTransform.TransformDirection(Vector3.up) * 8f;
        Debug.DrawRay(modelTransform.position, Upwards, Color.green);
    }
}
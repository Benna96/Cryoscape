using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform transformToFollow;
    [SerializeField] private Transform playerModel;
    [SerializeField] private float deltaHorizontalSensitivity = 75f;
    [SerializeField] private float deltaVerticalSensitivity = 75f;
    [SerializeField] private float continuousHorizontalSensitivity = 75f;
    [SerializeField] private float continuousVerticalSensitivity = 75f;

    private Vector2 deltaLookInput;
    private Vector2 continuousLookInput;
    private Vector2 lookDirection = Vector2.zero;

    private void Start() 
    {
        LockCursor();
    }

    private void Update()
    {
        cameraTransform.position = transformToFollow.position;

        if (deltaLookInput != Vector2.zero || continuousLookInput != Vector2.zero)
            RotatePlayerAndCamera();
    }


    private void RotatePlayerAndCamera()
    {
        // https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e

        if (deltaLookInput != Vector2.zero)
        {
            lookDirection.x += deltaLookInput.x * deltaHorizontalSensitivity;
            lookDirection.y += deltaLookInput.y * deltaVerticalSensitivity;
        }
        if (continuousLookInput != Vector2.zero)
        {
            lookDirection.x += continuousLookInput.x * continuousHorizontalSensitivity * Time.deltaTime;
            lookDirection.y += continuousLookInput.y * continuousVerticalSensitivity * Time.deltaTime;
        }
        lookDirection.y = Mathf.Clamp(lookDirection.y, -90f, 90f);

        var xQuat = Quaternion.AngleAxis(lookDirection.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(lookDirection.y, Vector3.left);

        playerModel.localRotation = xQuat;
        cameraTransform.localRotation = xQuat * yQuat;
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    /// <param name="context"></param>
    public void LookDelta(InputAction.CallbackContext context)
    {
        deltaLookInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    /// <param name="context"></param>
    public void LookContinuous(InputAction.CallbackContext context)
    {
        continuousLookInput = context.ReadValue<Vector2>();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}

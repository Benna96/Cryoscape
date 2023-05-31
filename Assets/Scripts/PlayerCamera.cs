using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            lookDirection.x += deltaLookInput.x * deltaHorizontalSensitivity * Time.deltaTime;
            lookDirection.y += deltaLookInput.y * deltaVerticalSensitivity * Time.deltaTime;
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

    public void OnLookDelta(InputValue value)
    {
        deltaLookInput = value.Get<Vector2>();
    }

    public void OnLookContinuous(InputValue value)
    {
        continuousLookInput = value.Get<Vector2>();
    }
}

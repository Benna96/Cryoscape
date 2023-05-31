using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform transformToFollow;
    [SerializeField] private Transform playerModel;
    [SerializeField] private float horizontalSensitivity = 75f;
    [SerializeField] private float verticalSensitivity = 75f;

    private Vector2 lookInput;
    private Vector2 lookDirection = Vector2.zero;

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        cameraTransform.position = transformToFollow.position;

        if (lookInput != Vector2.zero)
            RotatePlayerAndCamera();
    }

    private void RotatePlayerAndCamera()
    {
        // https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e

        lookDirection.x += lookInput.x * horizontalSensitivity * Time.deltaTime;
        lookDirection.y += lookInput.y * verticalSensitivity * Time.deltaTime;
        lookDirection.y = Mathf.Clamp(lookDirection.y, -90f, 90f);

        var xQuat = Quaternion.AngleAxis(lookDirection.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(lookDirection.y, Vector3.left);

        playerModel.localRotation = xQuat;
        cameraTransform.localRotation = xQuat * yQuat;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}

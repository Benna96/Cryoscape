using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InteractableManager : MonoBehaviour
{
    private new Camera camera;

    private Interactable _currentInteractable = null;
    private Interactable currentInteractable
    {
        get => _currentInteractable;
        set
        {
            if (value == _currentInteractable)
                return;

            if (_currentInteractable != null)
                _currentInteractable.DisableOutline();
            if (value != null)
                value.EnableOutline();

            _currentInteractable = value;
        }
    }
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // https://answers.unity.com/questions/1558578/how-do-you-find-an-object-in-the-middle-of-the-scr.html
        var ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);

        if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject.TryGetComponent<Interactable>(out var interactable))
            currentInteractable = interactable;
        else
            currentInteractable = null;
    }

    public void InteractWithCurrent()
    {
        if (currentInteractable != null)
            currentInteractable.Interact();
    }
}

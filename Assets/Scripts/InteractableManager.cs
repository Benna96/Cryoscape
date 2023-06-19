using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableManager : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword. If I add the keyword, Unity complains lol
    [SerializeField] private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    [SerializeField] private float maxInteractDistance;

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

    private void FixedUpdate()
    {
        // https://answers.unity.com/questions/1558578/how-do-you-find-an-object-in-the-middle-of-the-scr.html
        var ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, camera.nearClipPlane));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);

        if (Physics.Raycast(ray, out var hit, maxInteractDistance)
            && hit.collider.gameObject.TryGetComponent<Interactable>(out var interactable)
            && interactable.isInteractable)
            currentInteractable = interactable;
        else
            currentInteractable = null;
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        if (currentInteractable == null)
            return;

        currentInteractable.Interact();
    }
}

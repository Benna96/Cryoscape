using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableManager : MonoBehaviour
{
    [SerializeField] private new Camera camera;

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

        if (Physics.Raycast(ray, out var hit, 4f)
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

        if (currentInteractable != null && RequiredNormalItemIsSelectedOrSpecialExists())
            currentInteractable.Interact();

        bool RequiredNormalItemIsSelectedOrSpecialExists()
        {
            if (currentInteractable.requiredItem == null)
                return true;

            if (currentInteractable.requiredItem.category == Item.Category.Normal)
                return InventoryManager.instance.currentItem?.item.id == currentInteractable.requiredItem.id;
            else if (currentInteractable.requiredItem.category == Item.Category.Special)
                return InventoryManager.instance.items.Where(inventoryItem => inventoryItem.item.id == currentInteractable.requiredItem.id).Any();

            else return false;
        }
    }
}

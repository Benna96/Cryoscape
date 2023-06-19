using UnityEngine;
using UnityEngine.InputSystem;

public class IceMakerSound : MonoBehaviour
{
    // Reference to the AudioSource component
    public AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component if not already assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Check if the player raycasts onto this object
        if (Mouse.current.leftButton.wasPressedThisFrame) // Change the input condition according to your game's logic
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // Change the camera reference according to your game's logic

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Check if the player has the Spacesuit and Gloves
                    if (!HasItem("Coffeepan"))
                    {
                        // Play the audio if the player doesn't have the items
                        audioSource.Play();
                    }
                }
            }
        }
    }

    private bool HasItem(string itemName)
    {
        // Get a reference to the inventory manager
        InventoryManager inventoryManager = InventoryManager.instance;

        // Check if the inventory manager exists
        if (inventoryManager != null)
        {
            // Iterate over the items in the inventory
            foreach (var item in inventoryManager.items)
            {
                // Check if the item has the specified internal name
                if (item.internalName == itemName)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

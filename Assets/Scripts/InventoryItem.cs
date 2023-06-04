using UnityEngine;

public class InventoryItem : Interactable
{
    [field: SerializeField] public Item item { get; private set; }

    public override void Interact()
    {
        InventoryManager.instance.AddItem(this);
        this.gameObject.SetActive(false);
    }
}

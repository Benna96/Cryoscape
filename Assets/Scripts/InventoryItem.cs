using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable
{
    [field: SerializeField] public Texture2D inventoryIcon { get; private set; }

    public override void Interact()
    {
        InventoryManager.instance.AddItem(this);
        this.gameObject.SetActive(false);
    }
}

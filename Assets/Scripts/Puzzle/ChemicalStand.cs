using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

using UnityEngine;

public class ChemicalStand : Interactable
{
    [field: SerializeField] public InventoryItem chemical { get; private set; }

    private void Start()
    {
        (InventoryManager.instance.items as INotifyCollectionChanged).CollectionChanged += InventoryManager_CollectionChanged;
        InventoryManager.instance.PropertyChanged += InventoryManager_PropertyChanged;
        chemical.PropertyChanged += HeldItem_PropertyChanged;
    }

    private void HeldItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(InventoryItem.item))
        {
            chemical.gameObject.SetActive(chemical.item != null);
            UpdateIsInteractable();
        }
    }

    private void InventoryManager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add
            && e.NewItems.Contains(chemical.item))
        {
            chemical.item = null;
        }
    }

    private void InventoryManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(InventoryManager.instance.currentItem)
            && InventoryManager.instance.currentItem != null)
        {
            UpdateIsInteractable();
        }
    }

    protected override IEnumerator DoInteract()
    {
        Item playerHeldItem = InventoryManager.instance.currentItem;
        InventoryManager.instance.RemoveItem(playerHeldItem);

        chemical.item = playerHeldItem;
        chemical.gameObject.SetActive(true);

        yield return base.DoInteract();
    }

    protected override bool ShouldBeInteractable()
        => base.ShouldBeInteractable()
        && chemical.item == null
        && (requiredItem != null
            ? InventoryManager.instance.currentItem.id == requiredItem.id
            : InventoryManager.instance.currentItem is Item_Chemical);
}

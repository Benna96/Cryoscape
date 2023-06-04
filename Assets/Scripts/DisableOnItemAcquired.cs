using System.Collections.Specialized;

using UnityEngine;

public class DisableOnItemAcquired : MonoBehaviour
{
    [SerializeField] private Item itemToDisableOn;

    private void Start()
    {
        (InventoryManager.instance.items as INotifyCollectionChanged).CollectionChanged += InventoryItemsChanged;
    }

    private void InventoryItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            foreach (InventoryItem newItem in e.NewItems)
                if (newItem.item.id == itemToDisableOn.id)
                {
                    (InventoryManager.instance.items as INotifyCollectionChanged).CollectionChanged -= InventoryItemsChanged;
                    gameObject.SetActive(false);
                    break;
                }
    }
}

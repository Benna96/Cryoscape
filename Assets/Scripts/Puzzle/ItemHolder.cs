using System.Collections;
using System.ComponentModel;
using System.Linq;

using UnityEngine;
using UnityEngine.Serialization;

public class ItemHolder : Interactable
{
    [Tooltip("Use this instead of required item if item can be one of many")]
    [field: SerializeField, FormerlySerializedAs("requiredItemOptions")]
    public Item[] allowedItems { get; protected set; }

    [field: FormerlySerializedAs("<chemical>k__BackingField")]
    [field: SerializeField] public InventoryItem heldItem { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        heldItem.OnInteractCompleted += DisableHeldItemsItem;
        heldItem.PropertyChanged += DisableOrEnableHeldItem;

        isInteractableConditions.Add(_ => heldItem.item == null);
        heldItem.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(InventoryItem.item)) UpdateIsInteractable(); };
        UpdateIsInteractable();

        successConditions.Add(RequiredItemOptionCondition);

        void DisableHeldItemsItem(Interactable sender, InteractEventArgs e)
        {
            if (e.successfulInteract)
            {
                heldItem.item = null;
                Debug.Log("held item set to null");
            }
        }

        void DisableOrEnableHeldItem(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InventoryItem.item))
            {
                heldItem.gameObject.SetActive(heldItem.item != null);
                UpdateIsInteractable();
                Debug.Log("updated");
            }
        }

        bool RequiredItemOptionCondition(Interactable _)
        {
            if (allowedItems.Length == 0)
                return true;

            return allowedItems.Any(item => item.category switch
            {
                Item.Category.Normal => InventoryManager.instance.currentItem?.id == item.id,
                Item.Category.Special => InventoryManager.instance.items.Where(inventoryItem => inventoryItem.id == item.id).Any(),
                _ => false
            });
        }
    }

    protected override IEnumerator DoInteract()
    {
        heldItem.item = InventoryManager.instance.RemoveCurrentItem();

        yield return base.DoInteract();
    }
}

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

        StartCoroutine(AddInventoryConditionStuff());

        void DisableHeldItemsItem(Interactable sender, InteractEventArgs e)
        {
            if (e.successfulInteract)
                heldItem.item = null;
        }

        void DisableOrEnableHeldItem(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InventoryItem.item))
            {
                if (heldItem.item == null)
                    heldItem.gameObject.SetActive(false);
                else
                    StartCoroutine(HideSetActiveThenShow());

                UpdateIsInteractable();

                IEnumerator HideSetActiveThenShow() // Avoids simpleanim issues
                {
                    var renderers = heldItem.gameObject.GetComponents<Renderer>().Where(renderer => renderer.enabled).ToList();
                    renderers.ForEach(renderer => renderer.enabled = false);
                    heldItem.gameObject.SetActive(true);
                    yield return null;
                    renderers.ForEach(renderer => renderer.enabled = true);
                }
            }
        }

        IEnumerator AddInventoryConditionStuff()
        {
            yield return new WaitUntil(() => InventoryManager.instance != null);

            successConditions.Add(AllowedItemsCondition);
            UpdateShouldFail();

            bool AllowedItemsCondition(Interactable _)
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
    }

    protected override IEnumerator DoInteract()
    {
        heldItem.item = InventoryManager.instance.RemoveCurrentItem();

        yield return base.DoInteract();
    }
}

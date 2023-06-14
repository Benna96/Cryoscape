using System.Collections;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.Serialization;

public class ItemHolder : Interactable
{
    [field: FormerlySerializedAs("<chemical>k__BackingField")]
    [field: SerializeField] public InventoryItem heldItem { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        heldItem.OnInteractCompleted += DisableHeldItemsItem;
        heldItem.PropertyChanged += DisableOrEnableHeldItem;

        void DisableHeldItemsItem(Interactable sender, InteractEventArgs e)
        {
            if (e.successfulInteract)
                heldItem.item = null;
        }

        void DisableOrEnableHeldItem(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InventoryItem.item))
            {
                heldItem.gameObject.SetActive(heldItem.item != null);
                UpdateIsInteractable();
            }
        }
    }

    protected override IEnumerator DoInteract()
    {
        heldItem.item = InventoryManager.instance.RemoveCurrentItem();

        yield return base.DoInteract();
    }

    protected override bool ShouldBeInteractable()
    {
        var foo = base.ShouldBeInteractable()
            && heldItem.item == null;
        Debug.Log(foo);
        return foo;
    }
}

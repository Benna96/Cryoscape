using System.Collections;
using System.ComponentModel;

using UnityEngine;

public class ChemicalStand : Interactable
{
    [field: SerializeField] public InventoryItem chemical { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        chemical.OnInteractCompleted += DisableChemicalItem;
        chemical.PropertyChanged += DisableOrEnableChemical;
        StartCoroutine(AddInventoryEventHandlers());

        void DisableChemicalItem(Interactable sender, InteractEventArgs e)
        {
            if (e.successfulInteract)
                chemical.item = null;
        }

        void DisableOrEnableChemical(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InventoryItem.item))
            {
                chemical.gameObject.SetActive(chemical.item != null);
                UpdateIsInteractable();
            }
        }

        IEnumerator AddInventoryEventHandlers()
        {
            yield return new WaitUntil(() => InventoryManager.instance != null);
            UpdateIsInteractable();

            InventoryManager.instance.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(InventoryManager.currentItem))
                    UpdateIsInteractable();
            };
        }
    }

    protected override IEnumerator DoInteract()
    {
        chemical.item = InventoryManager.instance.RemoveCurrentItem();

        yield return base.DoInteract();
    }

    protected override bool ShouldBeInteractable()
        => base.ShouldBeInteractable()
        && chemical.item == null
        && InventoryManager.instance != null
        && InventoryManager.instance.currentItem != null
        && (requiredItem != null
            ? InventoryManager.instance.currentItem.id == requiredItem.id
            : InventoryManager.instance.currentItem is Item_Chemical);
}

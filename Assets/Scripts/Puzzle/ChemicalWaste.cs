using System.Collections;
using System.Linq;

using UnityEngine;

public class ChemicalWaste : Interactable
{
    [Header("Chemical waste variables")]

    [Tooltip("Chemicals you hsouldn't be able to waste. Namely, the base chemicals. Empty beaker (given below) will also be auto excluded.")]
    [SerializeField] private Item_Chemical[] noWasteChemicals;

    [SerializeField] private Item_Chemical emptyBeaker;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(AddInventoryEventHandlers());

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
        InventoryManager.instance.ReplaceCurrentItemWith(emptyBeaker);
        yield return StartCoroutine(base.DoInteract());
    }

    protected override bool ShouldBeInteractable()
        => base.ShouldBeInteractable()
        && InventoryManager.instance != null
        && InventoryManager.instance.currentItem is Item_Chemical
        && !noWasteChemicals.Contains(InventoryManager.instance.currentItem)
        && InventoryManager.instance.currentItem != emptyBeaker;
}

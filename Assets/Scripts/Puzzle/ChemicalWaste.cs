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

        successConditions.Add(CurrentItemIsAllowedCondition);
        StartCoroutine(UpdateShouldFailOnCurrentItemChanged());

        bool CurrentItemIsAllowedCondition(Interactable _)
            => InventoryManager.instance != null
            && InventoryManager.instance.currentItem is Item_Chemical
            && !noWasteChemicals.Contains(InventoryManager.instance.currentItem)
            && InventoryManager.instance.currentItem != emptyBeaker;

        IEnumerator UpdateShouldFailOnCurrentItemChanged()
        {
            yield return new WaitUntil(() => InventoryManager.instance != null);

            InventoryManager.instance.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(InventoryManager.currentItem))
                    UpdateShouldFail();
            };
            UpdateShouldFail();
        }
    }

    protected override IEnumerator DoInteract()
    {
        InventoryManager.instance.ReplaceCurrentItemWith(emptyBeaker);
        yield return StartCoroutine(base.DoInteract());
    }
}

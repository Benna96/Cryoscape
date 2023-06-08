using System;
using System.Collections;
using System.Linq;

using UnityEngine;

public class ChemicalMixer : MonoBehaviour
{
    [Tooltip("To use for generating chemicals")]
    [SerializeField] private GameObject generatedChemicalPrefab;

    [SerializeField] private ChemicalHolder[] chemicalStands;
    [SerializeField] private ChemicalHolder resultStand;
    [SerializeField] private Activatable button;

    [SerializeField] private Item[] allowedIngredients;
    [SerializeField] private Recipe[] recipes;
    [SerializeField] private GameObject inventoryitems; // debug only

    [Serializable]
    public class Recipe
    {
        public Item[] requiredIngredients;
        public Item resultingItem;
    }

    private void Awake()
    {
        Array.ForEach(chemicalStands, stand => stand.PropertyChanged += Stand_PropertyChanged);
        resultStand.PropertyChanged += Stand_PropertyChanged;
        button.PropertyChanged += Button_PropertyChanged;
        Array.ForEach(recipes, recipe => recipe.requiredIngredients = recipe.requiredIngredients.OrderBy(x => x.id).ToArray());
    }

    private void Stand_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ItemHolder.heldItem))
            UpdateShouldFail();
    }

    private void UpdateShouldFail()
        => button.shouldFail = resultStand.heldItem == null || !chemicalStands.Any(x => x.heldItem != null);

    private void Button_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Activatable.isActivated) && button.isActivated && resultStand.heldItem != null)
            Mix();
    }

    private void Start()
    {
        Debug.Log($"Giving player items");
        foreach (var inventoryItem in inventoryitems.GetComponentsInChildren<InventoryItem>())
        {
            inventoryItem.Interact();
            inventoryItem.transform.SetParent(null);
        }

        UpdateShouldFail();
    }

    private void Mix()
    {
        var ingredients = chemicalStands.Where(x => x.heldItem != null).Select(x => x.heldItem.item).OrderBy(x => x.id);
        Recipe matchingRecipe = recipes.Where(x => Enumerable.SequenceEqual(x.requiredIngredients.OrderBy(x => x.id), ingredients)).FirstOrDefault();

        var generated = Instantiate(generatedChemicalPrefab, resultStand.transform.position, resultStand.transform.rotation).GetComponent<InventoryItem>();
        if (matchingRecipe != null)
            generated.item = matchingRecipe.resultingItem;
    }
}

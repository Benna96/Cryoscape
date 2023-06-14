using System;
using System.Linq;

using UnityEngine;

public class ChemicalMixer : MonoBehaviour
{
    [SerializeField] private ChemicalStand[] chemicalStands;
    [SerializeField] private ChemicalStand resultStand;
    [SerializeField] private Activatable button;

    [SerializeField] private Item_Chemical nonRecipeChemical;
    [SerializeField] private Recipe[] recipes;

    // Debug only
    [SerializeField] private bool doAddItems;
    [SerializeField] private Item[] itemsToAddToInventory;

    [Header("Old and unneeded")]

    [Tooltip("To use for generating chemicals")]
    [SerializeField] private GameObject generatedChemicalPrefab;

    [Serializable]
    public class Recipe
    {
        public Item[] requiredIngredients;
        public Item resultingItem;
    }

    private void Awake()
    {
        button.PropertyChanged += Button_PropertyChanged;
        Array.ForEach(recipes, recipe => recipe.requiredIngredients = recipe.requiredIngredients.OrderBy(x => x.id).ToArray());

        button.successConditions.Add(_ => resultStand.heldItem.item != null && chemicalStands.Where(x => x.heldItem.item != null).Count() >= 2);


        void Button_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Activatable.isActivated) && !button.shouldFail)
                Mix();
        }
    }

    private void Start()
    {
        if (doAddItems)
        {
            Debug.Log($"Giving player items");
            foreach (var item in itemsToAddToInventory)
                InventoryManager.instance.AddItem(item);
        }
    }

    private void Mix()
    {
        var ingredients = chemicalStands.Where(x => x.heldItem.item != null).Select(x => x.heldItem.item).OrderBy(x => x.id);
        Recipe matchingRecipe = recipes.Where(x => Enumerable.SequenceEqual(x.requiredIngredients.OrderBy(x => x.id), ingredients)).FirstOrDefault();

        resultStand.heldItem.item = matchingRecipe?.resultingItem ?? nonRecipeChemical;
    }
}

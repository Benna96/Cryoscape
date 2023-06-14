using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Serialization;

public class ItemCombiner : MonoBehaviour
{
    [FormerlySerializedAs("chemicalStands")]
    [SerializeField] private ItemHolder[] inputItemHolders;

    [FormerlySerializedAs("resultStand")]
    [SerializeField] private ItemHolder outputItemHolder;

    [SerializeField] private Activatable button;

    [FormerlySerializedAs("nonRecipeChemical")]
    [SerializeField] private Item nonRecipeFallback;
    [SerializeField] private Recipe[] recipes;

    // Debug only
    [SerializeField] private bool doAddItems;
    [SerializeField] private Item[] itemsToAddToInventory;

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

        button.successConditions.Add(_ 
            => outputItemHolder.heldItem.item != null
            && inputItemHolders.Where(x => x.heldItem.item != null).Count() >= Mathf.Min(2, inputItemHolders.Count()));


        void Button_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Activatable.isActivated) && button.isActivated && !button.shouldFail)
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
        var ingredients = inputItemHolders.Where(x => x.heldItem.item != null).Select(x => x.heldItem.item).OrderBy(x => x.id);
        Recipe matchingRecipe = recipes.Where(x => Enumerable.SequenceEqual(x.requiredIngredients.OrderBy(x => x.id), ingredients)).FirstOrDefault();

        outputItemHolder.heldItem.item = matchingRecipe?.resultingItem ?? nonRecipeFallback ?? outputItemHolder.heldItem.item;
    }
}

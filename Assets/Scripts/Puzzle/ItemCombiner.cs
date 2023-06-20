using System;
using System.Collections;
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

    [Tooltip("Anims to play regardless of what is being combined. WIll be played at the same time as recipe specific anims.")]
    [SerializeField] private SimpleAnim[] combineAnims;

    [Tooltip("Same as above, but for audio.")]
    [SerializeField] private AudioSource[] combineAudio;

    [FormerlySerializedAs("nonRecipeChemical")]
    [SerializeField] private Item nonRecipeFallback;
    [SerializeField] private Recipe[] recipes;

    // Debug only
    [Header("For debug")]
    [SerializeField] private bool doAddItems;
    [SerializeField] private Item[] itemsToAddToInventory;

    private bool isMixing;
    protected float combineDuration;

    [Serializable]
    public class Recipe
    {
        public Item[] requiredIngredients;
        public Item resultingItem;
        public SimpleAnim[] anims;
        public AudioSource[] audio;
    }

    private void Awake()
    {
        combineDuration = combineAnims.Length == 0 ? 0f : combineAnims.Select(x => x.AnimDuration).Max();

        if (button != null)
        {
            button.PropertyChanged += Button_PropertyChanged;
            button.successConditions.Add(_
                => outputItemHolder.heldItem.item != null
                && inputItemHolders.Where(x => x.heldItem.item != null).Count() >= Mathf.Min(2, inputItemHolders.Count()));

            outputItemHolder.heldItem.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(InventoryItem.item)) button.UpdateShouldFail(); };
            Array.ForEach(inputItemHolders, holder => holder.heldItem.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(InventoryItem.item)) button.UpdateShouldFail(); });
        }

        else if (inputItemHolders.Count() == 1)
        {
            inputItemHolders[0].heldItem.PropertyChanged += AutocombineSingleItem;
        }

        Array.ForEach(recipes, recipe => recipe.requiredIngredients = recipe.requiredIngredients.OrderBy(x => x.id).ToArray());

        void Button_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Activatable.isActivated) && button.isActivated && !button.shouldFail)
            {
                StartCoroutine(button.MarkAsAnimatingFor(combineDuration));
                Combine();
            }
        }

        void AutocombineSingleItem(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!isMixing)
                Combine();
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

    private void Combine()
    {
        StartCoroutine(Inner());

        IEnumerator Inner()
        {
            isMixing = true;

            var ingredients = inputItemHolders.Where(x => x.heldItem.item != null).Select(x => x.heldItem.item).OrderBy(x => x.id);
            Recipe matchingRecipe = recipes.Where(x => Enumerable.SequenceEqual(x.requiredIngredients.OrderBy(x => x.id), ingredients)).FirstOrDefault();
            float recipeDuration = (matchingRecipe?.anims.Length == 0 ? 0f : matchingRecipe?.anims.Select(x => x.AnimDuration).Max()) ?? 0f;
            float totalDuration = Mathf.Max(combineDuration, recipeDuration);

            outputItemHolder.heldItem.item = matchingRecipe?.resultingItem ?? nonRecipeFallback ?? outputItemHolder.heldItem.item;
            StartCoroutine(outputItemHolder.heldItem.MarkAsAnimatingFor(totalDuration));

            Array.ForEach(combineAnims.Concat(matchingRecipe?.anims ?? Enumerable.Empty<SimpleAnim>()).ToArray(), anim =>
            {
                if (anim.gameObject.activeInHierarchy)
                    StartCoroutine(anim.AnimateNormal());
            });
            Array.ForEach(combineAudio.Concat(matchingRecipe?.audio ?? Enumerable.Empty<AudioSource>()).ToArray(), audio =>
            {
                if (audio != null)
                    audio.Play();
            });

            yield return new WaitForSeconds(totalDuration);

            isMixing = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    private VisualElement hotbarContainer;

    private VisualElement itemsContainer;
    private VisualElement[] items = new VisualElement[9];
    private VisualElement[] itemIcons = new VisualElement[9];
    private string[] itemNames = new string[9];

    private VisualElement itemNameContainer;
    private Label itemName;

    private Dictionary<Item, VisualElement> specialItemIcons = new();
    private bool setSpecialIconsFinished = false;

    private void OnEnable()
    {
        FetchElements(out var specialItemElements);
        SetLabels();
        SetSpecialIcons(specialItemElements);

        void FetchElements(out List<VisualElement> specialItemElements)
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            hotbarContainer = root.Q("HotbarContainer");

            itemNameContainer = root.Q("ItemNameContainer");
            itemName = itemNameContainer.Q<Label>();

            itemsContainer = root.Q("ItemsContainer");
            items = itemsContainer.Children().Select(x => x.Q("ItemWrapper")).ToArray();
            itemIcons = items.Select(x => x.Q("Icon")).ToArray();

            specialItemElements = root.Q("Special").Query(className: "item").ToList();
        }

        void SetLabels()
        {
            var itemLabels = items.Select(x => x.Q<Label>());

            foreach (var (label, index) in itemLabels.Select((x, i) => (x, i)))
                label.text = (index + 1).ToString();
        }

        void SetSpecialIcons(List<VisualElement> specialItemElements)
        {
            StartCoroutine(Inner(specialItemElements));
            IEnumerator Inner(List<VisualElement> specialItemElements)
            {
                specialItemElements.ForEach(x => x.SetEnabled(false));
                yield return new WaitUntil(() => InventoryManager.instance != null);

                foreach (var specialItemElement in specialItemElements)
                {
                    var correspondingItem = InventoryManager.instance.specialItemRefs.Where(itemRef => specialItemElement.name == itemRef.internalName).FirstOrDefault();
                    if (correspondingItem == null)
                        continue;

                    specialItemIcons.Add(correspondingItem, specialItemElement);
                    specialItemElement.style.backgroundImage = correspondingItem.inventoryIcon;
    }

                setSpecialIconsFinished = true;
            }
        }
    }

    private void Start()
    {
        // InventoryManager.instance may not exist OnEnable, so use it in Start instead
        RegisterEventsAndRunRelatedInitFuncs();

        void RegisterEventsAndRunRelatedInitFuncs()
        {
            (InventoryManager.instance.items as INotifyCollectionChanged).CollectionChanged += (_, e) => UpdateIcons(e);
            UpdateIcons();

            InventoryManager.instance.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(InventoryManager.instance.currentIndex))
                    FocusItem(InventoryManager.instance.currentCategory, InventoryManager.instance.currentIndex);
            };
            FocusItem(InventoryManager.instance.currentCategory, InventoryManager.instance.currentIndex);
        }
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    /// <param name="context"></param>
    public void ChangeItem(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        float direction = Mathf.Sign(context.ReadValue<float>());
        if (direction > 0)
            InventoryManager.instance.SelectNextItem();
        else if (direction < 0)
            InventoryManager.instance.SelectPreviousItem();
    }

    /// <summary>
    /// Called by Player Input
    /// </summary>
    /// <param name="context"></param>
    public void SelectItem(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        int index = int.Parse(Regex.Match(context.action.name, @"\d+$").Value) - 1;
        InventoryManager.instance.SelectItem(Item.Category.Normal, index);
    }

    private void UpdateIcons(NotifyCollectionChangedEventArgs e = null)
    {
        UpdateHotbar();
        UpdateSpecials();

        void UpdateHotbar()
        {
            var inventoryItems = InventoryManager.instance.GetItemsOfType(Item.Category.Normal);
            bool alreadyHadHotbarItems = !items[0].ClassListContains("dontshow");

            for (int i = 0; i < items.Length; i++)
            {
                if (i < inventoryItems.Count)
                {
                    itemIcons[i].style.backgroundImage = inventoryItems[i].item.inventoryIcon;
                    items[i].RemoveFromClassList("dontshow");
                    itemNames[i] = item.displayName;
                }
                else
                {
                    items[i].AddToClassList("dontshow");
                    itemIcons[i].style.backgroundImage = null;
                    itemNames[i] = null;
                }
            }

            if (inventoryItems.Count > 0)
            {
                hotbarContainer.RemoveFromClassList("dontshow");
                if (!alreadyHadHotbarItems || e == null)
                    InventoryManager.instance.SelectItem(Item.Category.Normal, 0);
            }
            else if (inventoryItems.Count == 0)
            {
                hotbarContainer.AddToClassList("dontshow");
            }
        }

        void UpdateSpecials()
        {
            if (e == null)
                StartCoroutine(SetAllEnabledStates());
            else
                SetEnabledStatesDependingOnChange();

            IEnumerator SetAllEnabledStates()
            {
                yield return new WaitUntil(() => setSpecialIconsFinished);

                var inventoryItems = InventoryManager.instance.GetItemsOfType(Item.Category.Special);
                foreach (var (item, element) in specialItemIcons)
                    element.SetEnabled(inventoryItems.Where(inventoryItem => inventoryItem.item == item).Any());
            }

            void SetEnabledStatesDependingOnChange()
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (InventoryItem addedItem in e.NewItems)
                        if (addedItem.item.category == Item.Category.Special && specialItemIcons.TryGetValue(addedItem.item, out var element))
                            element.SetEnabled(true);
                }

                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (InventoryItem removedItem in e.OldItems)
                        if (removedItem.item.category == Item.Category.Special && specialItemIcons.TryGetValue(removedItem.item, out var element))
                            element.SetEnabled(false);
                }
            }
        }
    }

    /// <summary>
    /// Only call in relation to current index changing!
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    private void FocusItem(Item.Category type, int index)
    {
        StartCoroutine(FocusAtEndOfFrame());

        IEnumerator FocusAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            if (type != Item.Category.Normal)
                yield break;

            if (index >= 0)
            {
                items[index].Focus();
                itemName.text = itemNames[index];
            }
        }
    }
}

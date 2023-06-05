using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    private VisualElement itemsContainer;
    private VisualElement[] items = new VisualElement[9];
    private VisualElement[] itemIcons = new VisualElement[9];

    private void OnEnable()
    {
        FetchElements();
        SetLabels();

        void FetchElements()
        {
            var ui = GetComponent<UIDocument>();
            itemsContainer = ui.rootVisualElement.Q("ItemsContainer");
            items = itemsContainer.Children().Select(x => x.Q("ItemWrapper")).ToArray();
            itemIcons = items.Select(x => x.Q("Icon")).ToArray();
        }

        void SetLabels()
        {
            var itemLabels = items.Select(x => x.Q<Label>());

            foreach (var (label, index) in itemLabels.Select((x, i) => (x, i)))
                label.text = (index + 1).ToString();
        }
    }

    private void Start()
    {
        // InventoryManager.instance may not exist OnEnable, so use it in Start instead
        RegisterEventsAndRunRelatedInitFuncs();

        void RegisterEventsAndRunRelatedInitFuncs()
        {
            (InventoryManager.instance.items as INotifyCollectionChanged).CollectionChanged += (_, _) => UpdateIcons();
            UpdateIcons();

            InventoryManager.instance.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(InventoryManager.instance.currentIndex))
                    FocusItem(InventoryManager.instance.currentCategory, InventoryManager.instance.currentIndex);
            };
            FocusItem(InventoryManager.instance.currentCategory, InventoryManager.instance.currentIndex);
        }
    }

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

    public void SelectItem(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
            return;

        int index = int.Parse(Regex.Match(context.action.name, @"\d+$").Value) - 1;
        InventoryManager.instance.SelectItem(Item.Category.Normal, index);
    }

    private void UpdateIcons()
    {
        var inventoryItems = InventoryManager.instance.GetItemsOfType(Item.Category.Normal);
        bool alreadyHadHotbarItems = !items[0].ClassListContains("dontshow");

        for (int i = 0; i < items.Length; i++)
        {
            if (i < inventoryItems.Count)
            {
                itemIcons[i].style.backgroundImage = inventoryItems[i].item.inventoryIcon;
                items[i].RemoveFromClassList("dontshow");
            }
            else
            {
                items[i].AddToClassList("dontshow");
                itemIcons[i].style.backgroundImage = null;
            }
        }

        if (inventoryItems.Count > 0)
        {
            itemsContainer.RemoveFromClassList("dontshow");
            if (!alreadyHadHotbarItems)
                InventoryManager.instance.SelectItem(Item.Category.Normal, 0);
        }
        else if (inventoryItems.Count == 0)
            itemsContainer.AddToClassList("dontshow");
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
            if (type == Item.Category.Normal && index > -1)
                items[index].Focus();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InventoryNonSpecialHolder : MonoBehaviour
{
    private List<Item> nonSpecialItems = new();
    private Item.Category category;
    private int index;

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || other.isTrigger)
            return;

        nonSpecialItems = InventoryManager.instance.items.Where(item => item.category != Item.Category.Special).ToList();
        category = InventoryManager.instance.currentCategory;
        index = InventoryManager.instance.currentIndex;

        nonSpecialItems.Reverse();
        nonSpecialItems.ForEach(item => InventoryManager.instance.RemoveItem(item));
        nonSpecialItems.Reverse();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || other.isTrigger)
            return;

        nonSpecialItems.ForEach(item => InventoryManager.instance.AddItem(item));
        InventoryManager.instance.SelectItem(category, index);

        nonSpecialItems.Clear();
        category = default;
        index = default;
    }
}

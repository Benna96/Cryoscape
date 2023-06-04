using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public ReadOnlyObservableCollection<InventoryItem> items { get; private set; }
    private ObservableCollection<InventoryItem> _items = new();

    public Item.Category currentCategory { get; private set; }

    private List<InventoryItem> currentCategoryItems => GetItemsOfType(currentCategory);

    private int _currentIndex = -1;
    public int currentIndex
    {
        get => _currentIndex;
        private set
        {
            _currentIndex = value;
            ObservableHelper.OnPropertyChanged(PropertyChanged);
        }
    }

    public InventoryItem currentItem => currentIndex >= 0 && currentIndex < currentCategoryItems.Count
        ? currentCategoryItems[currentIndex]
        : null;

    protected override void Awake()
    {
        base.Awake();

        items = new(_items);
        (items as INotifyCollectionChanged).CollectionChanged += InventoryManager_CollectionChanged;
    }

    private void InventoryManager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        Debug.Log($"Collection changed! Action: {e.Action}");
    }

    public List<InventoryItem> GetItemsOfType(Item.Category type) => items.Where(x => x.item.category == type).ToList();

    public void AddItem(InventoryItem item) => _items.Add(item);
    public void RemoveItem(InventoryItem item) => _items.Remove(item);

    public void SelectItem(Item.Category type, int index)
    {
        Debug.Log($"Selecting item of type {type} at {index}");

        var itemsOfType = GetItemsOfType(type);
        if (index >= itemsOfType.Count)
            index = itemsOfType.Count - 1;

        currentCategory = type;
        currentIndex = index;
    }
    public void SelectNextItem() { }
    public void SelectPreviousItem() { }
}

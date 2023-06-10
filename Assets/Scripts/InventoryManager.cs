using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System;

using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [field: SerializeField] public Item[] specialItemRefs { get; private set; }

    public ReadOnlyObservableCollection<Item> items { get; private set; }
    private ObservableCollection<Item> _items = new();

    public Item.Category currentCategory { get; private set; }

    private List<Item> currentCategoryItems => GetItemsOfType(currentCategory);

    private int _currentIndex = -1;
    public int currentIndex
    {
        get => _currentIndex;
        private set
        {
            _currentIndex = value;
            ObservableHelper.OnPropertyChanged(PropertyChanged);

            UpdateCurrentItem();
        }
    }

    private Item _currentItem;
    public Item currentItem
    {
        get => _currentItem;
        private set
        {
            if (value == _currentItem)
                return;

            _currentItem = value;
            ObservableHelper.OnPropertyChanged(PropertyChanged);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        items = new(_items);
        (items as INotifyCollectionChanged).CollectionChanged += InventoryManager_CollectionChanged;
    }

    private void InventoryManager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Contains(currentItem))
        {
            if (currentIndex >= currentCategoryItems.Count)
                currentIndex = currentCategoryItems.Count - 1;
            else
                UpdateCurrentItem();
        }
    }

    public List<Item> GetItemsOfType(Item.Category type) => items.Where(item => item.category == type).ToList();

    public void AddItem(Item item) => _items.Add(item);
    public void RemoveItem(Item item) => _items.Remove(item);

    public void SelectItem(Item.Category type, int index)
    {
        currentCategory = type;
        currentIndex = Mathf.Clamp(index, 0, currentCategoryItems.Count - 1);
    }
    public void SelectNextItem() => currentIndex = Mathf.Clamp(currentIndex + 1, 0, currentCategoryItems.Count - 1);
    public void SelectPreviousItem() => currentIndex = Mathf.Clamp(currentIndex - 1, 0, currentCategoryItems.Count - 1);

    private void UpdateCurrentItem()
    {
        if (currentIndex < 0)
            currentItem = null;
        else if (currentIndex >= currentCategoryItems.Count)
            throw new IndexOutOfRangeException("Current index higher than current category item count!");

        else
            currentItem = currentCategoryItems[currentIndex];
    }
}

﻿using System.Collections.Generic;
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

    private Item.Category _currentCategory;
    public Item.Category currentCategory
    {
        get => _currentCategory;
        private set
        {
            _currentCategory = value;
            UpdateCurrentIndexWithinWholeCollection();
        }
    }

    private List<Item> currentCategoryItems => GetItemsOfType(currentCategory);

    private int _currentIndex = -1;
    public int currentIndex
    {
        get => _currentIndex;
        private set
        {
            _currentIndex = Mathf.Clamp(value, 0, currentCategoryItems.Count - 1);
            if (currentCategoryItems.Count == 0)
                _currentIndex = -1;

            ObservableHelper.OnPropertyChanged(PropertyChanged);
            UpdateCurrentIndexWithinWholeCollection();
            UpdateCurrentItem();
        }
    }

    private int currentIndexWithinWholeCollection { get; set; }

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
        if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            if (e.OldItems.Count > 1)
                throw new NotImplementedException("Removing more than 1 item at a time from inventory is currently not supported!");

            else if (e.OldStartingIndex <= currentIndexWithinWholeCollection)
                currentIndex--;
        }
    }

    public List<Item> GetItemsOfType(Item.Category type) => items.Where(item => item.category == type).ToList();

    public void AddItem(Item item) => _items.Add(item);

    public Item RemoveItem(Item item)
    {
        if (item == currentItem)
            RemoveCurrentItem();
        else
            _items.Remove(item);

        return item;
    }

    public Item RemoveCurrentItem()
    {
        var oldItem = currentItem;
        _items.RemoveAt(currentIndexWithinWholeCollection);
        return oldItem;
    }

    public void ReplaceItem(Item oldItem, Item newItem)
    {
        if (oldItem == currentItem)
            ReplaceCurrentItemWith(newItem);
        else
            _items[_items.IndexOf(oldItem)] = newItem;
    }

    public void ReplaceCurrentItemWith(Item newItem)
    {
        _items[currentIndexWithinWholeCollection] = newItem;
        currentItem = newItem;
    }

    public void SelectItem(Item.Category type, int index)
    {
        currentCategory = type;
        currentIndex = index;
    }
    public void SelectNextItem() => currentIndex++;
    public void SelectPreviousItem() => currentIndex--;

    private void UpdateCurrentItem()
    {
        if (currentIndex < 0)
            currentItem = null;
        else if (currentIndex >= currentCategoryItems.Count)
            throw new IndexOutOfRangeException("Current index higher than current category item count!");

        else
            currentItem = currentCategoryItems[currentIndex];
    }

    private void UpdateCurrentIndexWithinWholeCollection()
    {
        if (currentIndex == -1)
        {
            currentIndexWithinWholeCollection = -1;
            return;
        }

        var item = currentCategoryItems[currentIndex];
        int nthInstance = currentCategoryItems.IndicesOf(item)
            .ToList()
            .IndexOf(currentIndex);

        currentIndexWithinWholeCollection = _items.IndicesOf(item).ElementAt(nthInstance);
    }
}

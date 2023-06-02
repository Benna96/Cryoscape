using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public ReadOnlyObservableCollection<InventoryItem> items { get; private set; }
    private ObservableCollection<InventoryItem> _items = new();

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

    public void AddItem(InventoryItem item) => _items.Add(item);
    public void RemoveItem(InventoryItem item) => _items.Remove(item);

    public void SelectItem(int index)
    {
        Debug.Log($"Selecting item {index}");
        if (index >= _items.Count)
            index = _items.Count - 1;

        currentIndex = index;
    }
    public void SelectNextItem() { }
    public void SelectPreviousItem() { }
}

using System.Collections.Specialized;
using System.ComponentModel;

using UnityEngine;

public class ItemHolder : Interactable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [field: SerializeField] public Transform heldItemPosition { get; private set; }

    private InventoryItem _heldItem;

    public InventoryItem heldItem
    {
        get => _heldItem;
        private set
        {
            _heldItem = value;
            ObservableHelper.OnPropertyChanged(PropertyChanged);
        }
    }

    private void Start()
    {
        (InventoryManager.instance.items as INotifyCollectionChanged).CollectionChanged += InventoryManager_CollectionChanged;
        InventoryManager.instance.PropertyChanged += InventoryManager_PropertyChanged;
    }

    private void InventoryManager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add
            && e.NewItems.Contains(heldItem))
        {
            heldItem = null;
            UpdateIsInteractable();
        }
    }

    private void InventoryManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        //Debug.Log($"{name}: inventory manager propoerty changed: {e.PropertyName}");
        if (e.PropertyName == nameof(InventoryManager.instance.currentItem)
            && InventoryManager.instance.currentItem != null)
        {
            Debug.Log($"Current item: {InventoryManager.instance.currentItem}");
            UpdateIsInteractable();
        }
    }

    public override void Interact()
    {
        InventoryItem playerHeldItem = InventoryManager.instance.currentItem;
        heldItem = playerHeldItem;

        playerHeldItem.gameObject.SetActive(false);
        playerHeldItem.gameObject.transform.SetPositionAndRotation(heldItemPosition.position, heldItemPosition.rotation);
        playerHeldItem.gameObject.SetActive(true);
        InventoryManager.instance.RemoveItem(playerHeldItem);
    }

    private void UpdateIsInteractable() => isInteractable = ShouldBeInteractable();
    protected virtual bool ShouldBeInteractable() => heldItem == null;
}

using System.ComponentModel;

using UnityEngine;

public class InventoryItem : Interactable, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    [SerializeField] private Item _item;
     public Item item
    {
        get => _item;
        set
        {
            _item = value;
            ObservableHelper.OnPropertyChanged(PropertyChanged);
        }
    }

    public override void Interact()
    {
        InventoryManager.instance.AddItem(this);
        this.gameObject.SetActive(false);
    }
}

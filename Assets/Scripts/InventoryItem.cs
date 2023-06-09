using System.Collections;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.Serialization;

public class InventoryItem : Interactable, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler PropertyChanged;

    [FormerlySerializedAs("<item>k__BackingField")]
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

    protected override IEnumerator DoInteract()
    {
        yield return StartCoroutine(base.DoInteract());
        InventoryManager.instance.AddItem(this);
        this.gameObject.SetActive(false);
    }
}

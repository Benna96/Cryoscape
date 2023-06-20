using System.Collections;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.Serialization;

public class InventoryItem : Interactable, INotifyPropertyChanged
{
    [FormerlySerializedAs("<item>k__BackingField")]
    [SerializeField] private Item _item;
     public Item item
    {
        get => _item;
        set
        {
            if (_item == value)
                return;

            _item = value;
            OnPropertyChanged();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        this.OnInteractCompleted += DisableOnPickup;

        void DisableOnPickup(Interactable sender, InteractEventArgs e)
        {
            if (e.successfulInteract)
                this.gameObject.SetActive(false);
        }
    }

    protected override IEnumerator DoInteract()
    {
        yield return StartCoroutine(base.DoInteract());
        InventoryManager.instance.AddItem(item);
    }
}

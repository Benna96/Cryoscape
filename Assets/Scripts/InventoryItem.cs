using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

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
            UpdateMaterialTextures();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        this.OnInteractCompleted += DisableOnPickup;

        UpdateMaterialTextures();

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
        item = null;
    }

    protected void UpdateMaterialTextures()
    {
        if (item == null || item.modelMaterialTextureOverrides == null || item.modelMaterialTextureOverrides.Length == 0)
            return;

        foreach (var texOverride in item.modelMaterialTextureOverrides)
        {
            var renderersWithMaterialToOverride = GetComponentsInChildren<Renderer>().Where(renderer => Regex.IsMatch(renderer.material.name, @$"^{texOverride.modelMainMaterial.name} \(Instance\)$")).ToArray();
            Array.ForEach(renderersWithMaterialToOverride, renderer => renderer.material.mainTexture = texOverride.modelMainTextureOverride);
        }
    }
}

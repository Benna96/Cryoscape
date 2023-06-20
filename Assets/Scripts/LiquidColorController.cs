using System;
using System.Collections;
using System.Linq;

using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class LiquidColorController : MonoBehaviour
{
    [SerializeField] private MeshRenderer liquidRenderer;

    [Tooltip("If multiple. Old left in to not break stuff.")]
    [SerializeField] private MeshRenderer[] liquidRenderers;

    private void OnValidate()
    {
        Item item = GetComponent<InventoryItem>().item;
        if (item != null && item is not IColoredItem)
            Debug.LogWarning($"{nameof(LiquidColorController)} will do nothing as long as InventoryItem isn't a colored item");
    }

    private void Awake()
    {
        GetComponent<InventoryItem>().PropertyChanged += InventoryItem_PropertyChanged;
    }

    private void Start()
    {
        UpdateColor();
    }

    private void InventoryItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(InventoryItem.item))
            UpdateColor();
    }

    private void UpdateColor()
    {
        IColoredItem coloredItem = GetComponent<InventoryItem>().item as IColoredItem;
        var color = coloredItem?.color ?? Color.clear;

        Array.ForEach(liquidRenderers.Append(liquidRenderer).ToArray(), renderer => renderer.material.color = color);
    }
}

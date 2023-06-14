using System.Collections;

using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class LiquidColorController : MonoBehaviour
{
    [SerializeField] private MeshRenderer liquidRenderer;

    private Color color;

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
        if (GetComponent<InventoryItem>().item is not IColoredItem chemicalItem)
            return;

        var color = chemicalItem.color;
        liquidRenderer.material.color = color;
    }
}

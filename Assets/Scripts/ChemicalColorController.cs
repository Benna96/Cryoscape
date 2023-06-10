using System.Collections;

using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class ChemicalColorController : MonoBehaviour
{
    [SerializeField] private MeshRenderer liquidRenderer;

    private Color color;

    private void OnValidate()
    {
        Item item = GetComponent<InventoryItem>().item;
        if (item != null && item is not Item_Chemical)
            Debug.LogWarning($"Component {nameof(ChemicalColorController)} requires InventoryItem's item to be a chemical item");
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
        var chemicalItem = GetComponent<InventoryItem>().item as Item_Chemical;
        if (chemicalItem == null)
            return;

        var color = chemicalItem.color;
        liquidRenderer.material.color = color;
    }
}

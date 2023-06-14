using UnityEngine;

public class CoffeePan : InventoryItem
{
    [Header("Coffee pan refs")]
    [SerializeField] private Item emptyPanRef;
    [SerializeField] private Item icePanRef;
    [SerializeField] private Item hotWaterPanRef;

    [Header("Coffee pan meshes")]
    [SerializeField] private GameObject iceMeshParent;
    [SerializeField] private GameObject hotWaterMeshParent;

    protected override void Awake()
    {
        base.Awake();

        PropertyChanged += ActivateCorrectMeshesOnItemChange;
        ActivateCorrectMeshes();
    }

    private void ActivateCorrectMeshesOnItemChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(item))
            ActivateCorrectMeshes();
    }

    private void ActivateCorrectMeshes()
    {
        if (item == null)
            return;

        iceMeshParent.SetActive(item.id == icePanRef.id);
        hotWaterMeshParent.SetActive(item.id == hotWaterPanRef.id);
    }
}

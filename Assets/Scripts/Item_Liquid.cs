using UnityEngine;

[CreateAssetMenu(fileName = "New chemical item", menuName = "Items/Liquids")]
public class Item_Liquid : Item, IColoredItem
{
    [field: SerializeField] public Color color { get; set; }

    [field: SerializeField] public Texture2D coloredInventoryIcon { get; set; }
}

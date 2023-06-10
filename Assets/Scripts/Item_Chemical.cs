using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New chemical item", menuName = "Items/Chemicals")]
public class Item_Chemical : Item, IColoredItem
{
    [field: FormerlySerializedAs("color")]
    [field: SerializeField] public Color color { get; set; }

    [field: SerializeField] public Texture2D coloredInventoryIcon { get; set; }
}

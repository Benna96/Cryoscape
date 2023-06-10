using System.Collections;

using UnityEngine;

public interface IColoredItem
{
    public Color color { get; set; }
    public Texture2D coloredInventoryIcon { get; set; }
}

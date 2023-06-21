using System.Collections;

using UnityEngine;

public interface IColoredItem
{
    public Color color { get; set; }

    [Tooltip("Will be rendered behind main icon")]
    public Texture2D coloredInventoryIcon { get; set; }
}

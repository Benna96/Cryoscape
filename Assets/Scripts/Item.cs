using System;
using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum Category
    {
        Normal,
        Special
    }

    public Guid id { get; protected set; }

    [Tooltip("Used for name-based matching.\nIf there's something here, don't change it, as it's likely used somewhere!")]
    public string internalName;

    public string displayName;
    public Category category = Category.Normal;
    public Texture2D inventoryIcon;

    protected Item() : base()
    {
        id = Guid.NewGuid();
    }
}

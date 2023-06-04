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
    public string displayName;
    public Category category = Category.Normal;
    public Texture2D inventoryIcon;

    protected Item() : base()
    {
        id = Guid.NewGuid();
    }
}

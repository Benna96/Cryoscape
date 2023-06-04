using System;
using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Guid id { get; protected set; }
    public string displayName;
    public Texture2D inventoryIcon;

    protected Item() : base()
    {
        id = Guid.NewGuid();
    }
}

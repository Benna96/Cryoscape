using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

﻿using QuickOutline;

using UnityEngine;

/// <summary>
/// Base Interactable class.
/// Inherit from this class & override Interact function to implement some kind of interaction.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    protected Outline outline;
    [field: SerializeField] public bool isInteractable { get; protected set; } = true;

    public List<Predicate<Interactable>> successConditions { get; private set; } = new();
    public bool shouldFail => successConditions.Any(condition => !condition(this));

    [field: SerializeField] public Item requiredItem { get; protected set; } = null;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        DisableOutline();

        successConditions.Add(RequiredItemCondition);

        bool RequiredItemCondition(Interactable dummy)
        {
            if (requiredItem == null)
                return true;

            return requiredItem.category switch
            {
                Item.Category.Normal => InventoryManager.instance.currentItem?.item.id == requiredItem.id,
                Item.Category.Special => InventoryManager.instance.items.Where(inventoryItem => inventoryItem.item.id == requiredItem.id).Any(),
                _ => false
            };
        }
    }

    public virtual void EnableOutline() => outline.enabled = true;

    public virtual void DisableOutline() => outline.enabled = false;

    /// <summary>
    /// Called when isInteractable is true.
    /// If there should be a failed interaction, handle it by checking for shouldFail.
    /// shouldFail is set to false if required item is missing.
    /// </summary>
    public abstract void Interact();
}

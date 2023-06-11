using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

﻿using QuickOutline;

using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Base Interactable class.
/// Inherit from this class & override Interact function to implement some kind of interaction.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    public delegate void InteractCompletedHandler(Interactable sender, InteractEventArgs e);
    public event InteractCompletedHandler OnInteractCompleted;

    protected Outline outline;
    [field: SerializeField] public bool isInteractable { get; protected set; } = true;

    public List<Predicate<Interactable>> successConditions { get; private set; } = new();
    public bool shouldFail => successConditions.Any(condition => !condition(this));

    [field: SerializeField] public Item requiredItem { get; protected set; } = null;

    [Header("Animations")]
    [SerializeField] protected bool disableInteractionWhileAnimating = false;

    [FormerlySerializedAs("activateAnims")]
    [Tooltip("Anims to play when interacting")]
    [SerializeField] protected SimpleAnim[] interactAnims;

    [FormerlySerializedAs("failedActivateAnims")]
    [Tooltip("Animations to play upon failed interact (not having required item or such)")]
    [SerializeField] protected SimpleAnim[] failedInteractAnims;

    protected float interactDuration;
    protected float failedInteractDuration;
    protected virtual float applicableInteractDuration => shouldFail ? failedInteractDuration
        : interactDuration;

    private bool interactBlockedByAnimation;

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        DisableOutline();

        successConditions.Add(RequiredItemCondition);

        interactDuration = GetMaxAnimDuration(interactAnims);
        failedInteractDuration = GetMaxAnimDuration(failedInteractAnims);

        UpdateIsInteractable();

        bool RequiredItemCondition(Interactable dummy)
        {
            if (requiredItem == null)
                return true;

            return requiredItem.category switch
            {
                Item.Category.Normal => InventoryManager.instance.currentItem?.id == requiredItem.id,
                Item.Category.Special => InventoryManager.instance.items.Where(inventoryItem => inventoryItem.id == requiredItem.id).Any(),
                _ => false
            };
        }

        static float GetMaxAnimDuration(SimpleAnim[] anims)
            => anims.Length == 0 ? 0f : anims.Select(x => x.AnimDuration).Max();
    }

    public virtual void EnableOutline() => outline.enabled = true;

    public virtual void DisableOutline() => outline.enabled = false;

    /// <summary>
    /// Called when isInteractable is true.
    /// If there should be a failed interaction, handle it by checking for shouldFail.
    /// shouldFail is set to false if required item is missing.
    /// </summary>
    public virtual void Interact()
    {
        StartCoroutine(InteractThenRaiseEvent());

        if (isInteractable && disableInteractionWhileAnimating)
            StartCoroutine(CoroutineHelper.StartWaitEnd(
                () => { interactBlockedByAnimation = true; UpdateIsInteractable(); },
                () => { interactBlockedByAnimation = false; UpdateIsInteractable(); },
                applicableInteractDuration));

        IEnumerator InteractThenRaiseEvent()
        {
            if (!shouldFail)
            {
                yield return StartCoroutine(DoInteract());
                OnInteractCompleted?.Invoke(this, new(true));
            }
            else
            {
                yield return StartCoroutine(DoFailedInteract());
                OnInteractCompleted?.Invoke(this, new(false));
            }
        }
    }

    protected virtual IEnumerator DoInteract()
    {
        Array.ForEach(interactAnims, anim => StartCoroutine(anim.AnimateNormal()));
        yield return new WaitForSeconds(interactDuration);
    }

    protected virtual IEnumerator DoFailedInteract()
    {
        Array.ForEach(failedInteractAnims, anim => StartCoroutine(anim.AnimateNormal()));
        yield return new WaitForSeconds(failedInteractDuration);
    }

    protected void UpdateIsInteractable() => isInteractable = ShouldBeInteractable();
    protected virtual bool ShouldBeInteractable() => !interactBlockedByAnimation;
}

public class InteractEventArgs : EventArgs
{
    public bool successfulInteract { get; private set; }

    public InteractEventArgs(bool success)
    {
        this.successfulInteract = success;
    }
}
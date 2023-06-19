using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

﻿using QuickOutline;

using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Base Interactable class.
/// Inherit from this class & override Interact function to implement some kind of interaction.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour, INotifyPropertyChanged
{
    public delegate void InteractCompletedHandler(Interactable sender, InteractEventArgs e);
    public event InteractCompletedHandler OnInteractCompleted;

    public event PropertyChangedEventHandler PropertyChanged;

    protected Outline outline;

    public List<Predicate<Interactable>> isInteractableConditions { get; private set; } = new();
    [field: SerializeField] public bool isInteractable { get; private set; } = true;

    public List<Predicate<Interactable>> successConditions { get; private set; } = new();
    public bool shouldFail { get; private set; }

    [field: SerializeField] public Item requiredItem { get; protected set; } = null;
    [field: SerializeField] public Activatable requiredActivatable { get; protected set; } = null;

    [Header("Animations")]
    [SerializeField] protected bool disableInteractionWhileAnimating = false;

    [FormerlySerializedAs("activateAnims")]
    [Tooltip("Anims to play when interacting")]
    [SerializeField] protected SimpleAnim[] interactAnims;
    [SerializeField] protected AudioSource[] interactAudios;

    [FormerlySerializedAs("failedActivateAnims")]
    [Tooltip("Animations to play upon failed interact (not having required item or such)")]
    [SerializeField] protected SimpleAnim[] failedInteractAnims;
    [SerializeField] protected AudioSource[] failedInteractAudios;

    protected float interactDuration;
    protected float failedInteractDuration;

    protected virtual float applicableInteractDuration => shouldFail ? failedInteractDuration
            : interactDuration;

    private bool _interactBlockedByAnimation;
    private bool interactBlockedByAnimation
    {
        get => _interactBlockedByAnimation;
        set
        {
            _interactBlockedByAnimation = value;
            OnPropertyChanged();
        }
    }

    private Coroutine markAsAnimatingForCoroutine;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        DisableOutline();

        isInteractableConditions.Add(_ => !disableInteractionWhileAnimating || !interactBlockedByAnimation);
        PropertyChanged += (_, e) => { if (e.PropertyName == nameof(interactBlockedByAnimation)) UpdateIsInteractable(); };

        StartCoroutine(AddInventoryConditionStuff());

        successConditions.Add(_ => requiredActivatable == null || requiredActivatable.isActivated);
        if (requiredActivatable != null)
            requiredActivatable.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(Activatable.isActivated)) UpdateShouldFail(); };

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

        IEnumerator AddInventoryConditionStuff()
        {
            yield return new WaitUntil(() => InventoryManager.instance != null);

            successConditions.Add(RequiredItemCondition);
            InventoryManager.instance.PropertyChanged += (_, e) => { if (e.PropertyName == nameof(InventoryManager.currentItem)) UpdateShouldFail(); };
            (InventoryManager.instance.items as INotifyCollectionChanged).CollectionChanged += (_, _) => UpdateShouldFail();

            UpdateShouldFail();
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
            StartCoroutine(MarkAsAnimatingFor(applicableInteractDuration));

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
        Array.ForEach(interactAudios, audio => audio.Play());
        yield return new WaitForSeconds(interactDuration);
    }

    protected virtual IEnumerator DoFailedInteract()
    {
        Array.ForEach(failedInteractAnims, anim => StartCoroutine(anim.AnimateNormal()));
        Array.ForEach(failedInteractAudios, audio => audio.Play());
        yield return new WaitForSeconds(failedInteractDuration);
    }

    public void UpdateIsInteractable() => isInteractable = isInteractableConditions.All(condition => condition(this));
    public void UpdateShouldFail()
    {
        shouldFail = successConditions.Any(condition => !condition(this));
        outline.OutlineColor = shouldFail ? Color.red : Color.white;
    }

    public IEnumerator MarkAsAnimatingFor(float seconds)
    {
        if (seconds == 0f || !isActiveAndEnabled)
            yield break;

        if (markAsAnimatingForCoroutine != null)
        {
            StopCoroutine(markAsAnimatingForCoroutine);
            yield return null;
        }

        markAsAnimatingForCoroutine = StartCoroutine(CoroutineHelper.StartWaitEnd(
            () => { interactBlockedByAnimation = true;},
            () => { interactBlockedByAnimation = false;},
            seconds));
        yield return markAsAnimatingForCoroutine;
        markAsAnimatingForCoroutine = null;
    }
}

public class InteractEventArgs : EventArgs
{
    public bool successfulInteract { get; private set; }

    public InteractEventArgs(bool success)
    {
        this.successfulInteract = success;
    }
}